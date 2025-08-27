using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Requests;
using Dotnet.Crm.Domain.SeedWork.ErrorResult;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;
using Dotnet.Crm.Domain.Aggregates.Crm;
using Dotnet.Crm.Domain.Aggregates.Crm.Entities.Responses;

namespace Dotnet.Crm.App.Extensions
{
    public static class EndpointsExtensions
    {
        private static readonly Regex EmojiRegex = new Regex
        (
            @"([\u2700-\u27BF]|[\uE000-\uF8FF]|[\uD83C-\uDBFF\uDC00-\uDFFF]|\u200D|\uFE0F)",
            RegexOptions.Compiled
        );

        public static void AddEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapGet("/health", Health);
            endpointRouteBuilder.MapPost("/v1/proposal", PostProposal);
            endpointRouteBuilder.MapPost("/v1/proposal/{id}/send-to-sign", PostProposalToSign);
            endpointRouteBuilder.MapPost("/v1/proposal/{id}/callbacks/signed", PostSignCallback);
            endpointRouteBuilder.MapGet("/v1/proposal/single", GetProposalById);
        }

        [SwaggerOperation(
             Summary = "Application Health Check Endpoint",
             Description = "This endpoint validates whether the application is working correctly according to internally established standards.",
             OperationId = "Health",
             Tags = ["Health"]
        )]
        public static IResult Health() => Results.Ok();

        [SwaggerOperation(
             Summary = "PostProposal - Inserts a new proposal into the service provider's proposal service.",
             Description = "This endpoint is responsible for utilizing the service provider's proposal service to insert a new proposal.",
             OperationId = "PostProposal",
             Tags = ["Proposal"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> PostProposal([FromBody] ProposalRequest proposal, [FromServices] ICrmService crmService)
        {
            var validateProposal = ValidateProposal(proposal);

            if (validateProposal is not null)
                return validateProposal;

            proposal.Items = MergeItemRequests(proposal.Items);

            var result = await crmService.InsertProposalAsync(proposal);

            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Created($"/v1/proposal/single/{result.Item1.Id}", result.Item1);
        }
        [SwaggerOperation(
             Summary = "PostProposal - Inserts a new proposal into the service provider's proposal service.",
             Description = "This endpoint is responsible for utilizing the service provider's proposal service to insert a new proposal.",
             OperationId = "PostProposal",
             Tags = ["Proposal"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> PostProposalToSign([FromRoute] Guid id, [FromServices] ICrmService crmService)
        {
            Tuple<ProposalResponse?, ErrorResult> result = await crmService.SendProposalToSign(id);

            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Created($"/v1/proposal/single/{result.Item1.Id}", result.Item1);
        }
        [SwaggerOperation(
             Summary = "PostProposal - Inserts a new proposal into the service provider's proposal service.",
             Description = "This endpoint is responsible for utilizing the service provider's proposal service to insert a new proposal.",
             OperationId = "PostProposal",
             Tags = ["Proposal"]
        )]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> PostSignCallback([FromRoute] Guid id, [FromServices] ICrmService crmService)
        {
            Tuple<ProposalResponse?, ErrorResult> result = await crmService.PostProposalSigned(id);

            if (result.Item1 is null || result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Created($"/v1/proposal/single/{result.Item1.Id}", result.Item1);
        }

        [SwaggerOperation(
             Summary = "GetProposalById - Retrieves a list of proposals filtered by the specified parameters.",
             Description = "This endpoint utilizes the service provider's proposal service to select a list of proposals, filtering them based on the parameters provided.",
             OperationId = "GetProposalById",
             Tags = ["Proposal"]
        )]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> GetProposalById([FromServices] ICrmService crmService, [FromRoute] Guid proposalId)
        {
            var result = await crmService.SelectProposalByIdAsync(proposalId);

            if (result.Item2 is not null && result.Item2.Error)
                return GenerateErrorResult(result.Item2);

            return Results.Ok(result.Item1);
        }

        private static List<ItemRequest> MergeItemRequests(List<ItemRequest> items)
        {
            return items
                .GroupBy(item => item.Name)
                .Select(group => new ItemRequest
                {
                    Name = group.Key,
                    Quantity = group.Sum(item => item.Quantity),
                    UnitPrice = group.First().UnitPrice
                })
                .ToList();
        }

        private static IResult? ValidateProposal(ProposalRequest proposal)
        {
            var errors = new List<string>();

            var emojiValidation = ValidateNoEmojis(proposal);
            if (emojiValidation.Count > 0)
            {
                errors.Add("Emojis are not allowed.");
            }

            var itemsErrors = ValidateItems(proposal.Items);
            if (itemsErrors.Any())
            {
                errors.AddRange(itemsErrors);
            }

            if (errors.Any())
            {
                return GenerateErrorResult(new ErrorResult
                {
                    Error = true,
                    Message = string.Join(" | ", errors),
                    StatusCode = ErrorCode.UnprocessableEntity,
                    Id = "validation-errors"
                });
            }

            return null;
        }

        private static List<string> ValidateItems(List<ItemRequest> items)
        {
            var errors = new List<string>();

            if (items == null || items.Count == 0)
            {
                errors.Add("At least one item is required.");
                return errors;
            }

            foreach (var item in items)
            {
                if (string.IsNullOrWhiteSpace(item.Name))
                {
                    errors.Add("Sku is required for all items.");
                }

                if (item.Quantity <= 0)
                {
                    errors.Add($"Quantity must be greater than 0 for item with SKU '{item.Name}'.");
                }

                if (item.UnitPrice <= 0)
                {
                    errors.Add($"UnitPrice must be greater than 0 for item with SKU '{item.Name}'.");
                }
            }

            return errors;
        }
        private static List<string> ValidateNoEmojis(object obj, string parentPath = "")
        {
            var errors = new List<string>();

            if (obj == null)
                return errors;

            Type type = obj.GetType();

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = prop.GetValue(obj);
                string propPath = string.IsNullOrEmpty(parentPath) ? prop.Name : $"{parentPath}.{prop.Name}";

                if (value is string strValue)
                {
                    if (EmojiRegex.IsMatch(strValue))
                    {
                        errors.Add($"Campo '{propPath}' contém emoji.");
                    }
                }
                else if (value is IEnumerable enumerable && !(value is string))
                {
                    int index = 0;
                    foreach (var item in enumerable)
                    {
                        errors.AddRange(ValidateNoEmojis(item, $"{propPath}[{index}]"));
                        index++;
                    }
                }
                else if (value != null && !prop.PropertyType.IsPrimitive && !prop.PropertyType.IsEnum && prop.PropertyType != typeof(DateTime))
                {
                    errors.AddRange(ValidateNoEmojis(value, propPath));
                }
            }

            return errors;
        }

        private static IResult GenerateErrorResult(ErrorResult errorResult) => errorResult.StatusCode switch
        {
            ErrorCode.Undefined => Results.Problem(
                detail: JsonConvert.SerializeObject(errorResult),
                statusCode: 500
            ),
            ErrorCode.NotFound => Results.NotFound(errorResult),
            ErrorCode.BadRequest => Results.BadRequest(errorResult),
            ErrorCode.Unauthorized => Results.Unauthorized(),
            ErrorCode.Forbidden => Results.Forbid(null),
            ErrorCode.InternalServerError => Results.Problem(
                detail: JsonConvert.SerializeObject(errorResult),
                statusCode: 500
            ),
            ErrorCode.UnprocessableEntity => Results.UnprocessableEntity(errorResult),
            _ => Results.Problem(
                detail: JsonConvert.SerializeObject(errorResult),
                statusCode: 422
            )
        };
    }
}
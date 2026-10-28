namespace LastMile.Api.Dtos;

public sealed record AssignCourierRequest(Guid CourierId, string RequestedBy);
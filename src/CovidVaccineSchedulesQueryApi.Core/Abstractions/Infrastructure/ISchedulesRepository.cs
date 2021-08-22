namespace CovidVaccineSchedulesQueryApi.Core.Abstractions.Infrastructure;

using System.Buffers;

public interface ISchedulesRepository
{
    ValueTask<ReadOnlySequence<byte[]>> GetAllAsync(DateOnly startDate, DateOnly endDate);

    ValueTask<ReadOnlySequence<byte>> GetByAsync(Guid personId);
}

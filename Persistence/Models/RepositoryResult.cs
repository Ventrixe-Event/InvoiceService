namespace Persistence.Models;

public class RepositoryResult<T>
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public T? Data { get; set; }

    public static RepositoryResult<T> Success(T data)
    {
        return new RepositoryResult<T> { IsSuccess = true, Data = data };
    }

    public static RepositoryResult<T> Failure(string errorMessage)
    {
        return new RepositoryResult<T> { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

public class RepositoryResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }

    public static RepositoryResult Success()
    {
        return new RepositoryResult { IsSuccess = true };
    }

    public static RepositoryResult Failure(string errorMessage)
    {
        return new RepositoryResult { IsSuccess = false, ErrorMessage = errorMessage };
    }
}

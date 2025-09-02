namespace BGT_Web_Contracts;


public interface UserCreated
{
    long UserId { get; }
    string UserName { get; }
    string Email { get; }

    DateTime CreatedAtUtc { get; }
}

public interface UserEmailVerified
{
    long UserId { get; }
    DateTime VerifiedAtUtc { get; }
}

public interface UserDeleted
{
    long UserId { get; }
    DateTime DeletedAtUtc { get; }
}

public interface UserExistsRequest
{
    int UserId { get; }
}

public interface UserExistsObjectResponse
{
    int UserId { get; }
    string UserName { get; }
    string Email { get; }
}

public interface UserExistsResponse
{
    bool Exists { get; }
    UserExistsObjectResponse? User { get; }
}

using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

internal class CommentIdentityConverter : IdentityConverter<CommentId>
{
    internal CommentIdentityConverter() : 
        base((x) => new CommentId(x))
    {
    }
}

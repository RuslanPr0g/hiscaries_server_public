using HC.Domain.Stories;

namespace HC.Infrastructure.Configurations.Converters;

public class CommentIdentityConverter : IdentityConverter<CommentId>
{
    public CommentIdentityConverter() : 
        base((x) => new CommentId(x))
    {
    }
}

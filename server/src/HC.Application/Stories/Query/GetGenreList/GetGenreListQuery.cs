using MediatR;
using System.Collections.Generic;

namespace HC.Application.Stories.Query.GetGenreList;

public sealed class GetGenreListQuery : IRequest<IEnumerable<GenreReadModel>>
{
}
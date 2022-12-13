using BooksApi.Models.Books;
using GraphQL.Execution;
using GraphQL.Types;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Api.Models
{
    public class BookApiQuery : ObjectGraphType
    {
        public BookApiQuery(IBookRepository _bookRepository)
        {
            Field<ListGraphType<BookType>>("books",
                arguments: new QueryArguments(new List<QueryArgument>
                {
                    new QueryArgument<StringGraphType>
                    {
                        Name = "originallyPublished",
                        Description = "Book originally published date."
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "author",
                        Description = "Book author's."
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "genre",
                        Description = "Book genre."
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "illustrator",
                        Description = "Book illustrator."
                    },
                    new QueryArgument<IntGraphType>
                    {
                        Name = "pageCount",
                        Description = "Book total page count."
                    },
                    new QueryArgument<StringGraphType>
                    {
                        Name = "sortByPrice",
                        Description = "Sort by price Ascending or Descending."
                    }
                }),
            resolve: context =>
            {
                var books = _bookRepository.GetBooks();

                ArgumentValue originallyPublished = new ArgumentValue();
                context.Arguments.TryGetValue("originallyPublished", out originallyPublished);
                if (!string.IsNullOrEmpty((string)originallyPublished.Value))
                {
                    books.RemoveAll(x => x.Specifications.OriginallyPublished != (string)originallyPublished.Value);
                }

                ArgumentValue author = new ArgumentValue();
                context.Arguments.TryGetValue("author", out author);
                if (!string.IsNullOrEmpty((string)author.Value))
                {
                    books.RemoveAll(x => x.Specifications.Author != (string)author.Value);
                }
                ArgumentValue pageCount = new ArgumentValue();
                context.Arguments.TryGetValue("pageCount", out pageCount);
                if (pageCount.Value != null && (int)pageCount.Value  > 0)
                {
                    books.RemoveAll(x => x.Specifications.PageCount != (int)pageCount.Value);
                }
                /*
                var genre = context.GetArgument<string>("genre");
                if (!string.IsNullOrEmpty(genre))
                {
                    books.RemoveAll(x => x.Specifications.Genres.Contains(genre) == false);
                }

                var illustrator = context.GetArgument<string>("illustrator");
                if (!string.IsNullOrEmpty(illustrator))
                {
                    books.RemoveAll(x => x.Specifications.Illustrator.Contains(illustrator) == false);
                }

                var sortByPrice = context.GetArgument<string>("sortByPrice");
                if (!string.IsNullOrEmpty(sortByPrice))
                {
                    if (sortByPrice == "Ascending")
                        books = books.OrderBy(x => x.Price).ToList();
                    if (sortByPrice == "Descending")
                        books = books.OrderByDescending(x => x.Price).ToList();
                }
                */
                return books.ToList();
            });
        }
    }
}
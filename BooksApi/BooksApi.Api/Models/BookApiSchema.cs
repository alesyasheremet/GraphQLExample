using GraphQL;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BooksApi.Api.Models
{
    public class BookApiSchema : Schema
    {
        public BookApiSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<BookApiQuery>();
        }

        
    }
}

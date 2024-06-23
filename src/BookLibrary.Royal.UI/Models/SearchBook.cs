using System.Text;

namespace BookLibrary.Royal.UI.Models;

public class SearchBook
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int? TotalCopies { get; set; }
    public int? CopiesInUse { get; set; }
    public string? Type { get; set; }
    public string? Isbn { get; set; }
    public string? Category { get; set; }

    public int OffSet { get; set; } = 0;
    public int Limit { get; set; } = 1000;

    public string ToQueryString()
    {
        var query = new StringBuilder();

        if (Id.HasValue)
            query.Append($"id={Id}&");

        if (!string.IsNullOrWhiteSpace(Title))
            query.Append($"title={Title}&");

        if (!string.IsNullOrWhiteSpace(FirstName))
            query.Append($"firstName={FirstName}&");

        if (!string.IsNullOrWhiteSpace(LastName))
            query.Append($"lastName={LastName}&");

        if (TotalCopies.HasValue)
            query.Append($"totalCopies={TotalCopies}&");

        if (CopiesInUse.HasValue)
            query.Append($"copiesInUse={CopiesInUse}&");

        if (!string.IsNullOrWhiteSpace(Type))
            query.Append($"type={Type}&");

        if (!string.IsNullOrWhiteSpace(Isbn))
            query.Append($"isbn={Isbn}&");

        if (!string.IsNullOrWhiteSpace(Category))
            query.Append($"category={Category}&");

        query.Append($"offSet={OffSet}&");
        query.Append($"limit={Limit}");

        return query.ToString();
    }
}

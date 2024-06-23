using System.Text;

namespace BookLibrary.Royal.Domain.Filter;

public class BookFilter
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

    public string BuildWhereClause(StringBuilder queryBuilder)
    {
        bool hasCondition = false;

        if (Id.HasValue)
        {
            queryBuilder.Append($" book_id = {Id.Value}");
            hasCondition = true;
        }

        if (!string.IsNullOrEmpty(Title))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"title LIKE '%{Title}%'");
            hasCondition = true;
        }

        if (!string.IsNullOrEmpty(FirstName))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"first_name LIKE '%{FirstName}%'");
            hasCondition = true;
        }
        if (!string.IsNullOrEmpty(LastName))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"last_name LIKE '%{LastName}%'");
            hasCondition = true;
        }

        if (TotalCopies.HasValue)
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"total_copies = {TotalCopies.Value}");
            hasCondition = true;
        }

        if (CopiesInUse.HasValue)
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"copies_in_use = {CopiesInUse.Value}");
            hasCondition = true;
        }

        if (!string.IsNullOrEmpty(Type))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"type LIKE '%{Type}%'");
            hasCondition = true;
        }

        if (!string.IsNullOrEmpty(Isbn))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"isbn LIKE '%{Isbn}%'");
            hasCondition = true;
        }

        if (!string.IsNullOrEmpty(Category))
        {
            if (hasCondition) queryBuilder.Append(" AND ");
            queryBuilder.Append($"category LIKE '%{Category}%'");
            hasCondition = true;
        }

        var limit = $" ORDER BY title OFFSET {OffSet} ROWS FETCH NEXT {Limit} ROWS ONLY";
        if (hasCondition)
            return "WHERE " + queryBuilder.ToString() + limit;

        return limit;
    }

    public bool HasAnyArgument()
    {
        return Id is not null ||
               Title is not null ||
               FirstName is not null ||
               LastName is not null ||
               TotalCopies is not null ||
               CopiesInUse is not null ||
               Type is not null ||
               Isbn is not null ||
               Category is not null;
    }

    public string GetKey()
    {
        return $"{Id}-{Title}-{FirstName}-{LastName}-{TotalCopies}-{CopiesInUse}-{Type}-{Isbn}-{Category}-{Limit}-{OffSet}";
    }
}

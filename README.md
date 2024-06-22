# BookLibrary.Royal

BookLibrary.Royal is an Api for books search.

## Installation

Required appsettings configuration

```bash
"ConnectionStrings": {
  "Book": "Server=localhost;Database=BookLibrary;User Id=sa;Password=iwannarock;TrustServerCertificate=True;MultipleActiveResultSets=true"
},
"Caching": {
  "TimeoutSeconds": 300
}
```

## Implementation

- It available swagger in order to make easy to make requests.
- It has a filter by properties adding limit and offset.
- Memory Caching with Decorator Pattern, in order to avoid going to database.
  - this should be replaced by a Distributed Cache like Couchbase or similar.
  - The cacheKey is based on properties informed, given the scenario it is the best approach in order to avoid missing information that hasn´t been cached yet.
- ObjectPool is used in order to avoid creating StringBuilde object, that is a large object. Improving memory allocation.
- All properties of Book is available to search at api.
- When querying against database application is using WITH (NOLOCK)
- Application uses Dapper in order to make it easy to query.
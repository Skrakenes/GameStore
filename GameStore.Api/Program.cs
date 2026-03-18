using GameStore.Api.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;

const string GetGameEndpointName = "GetGame";

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<GameDto> games = [
    new (
        1, 
        "Street Fighter II", 
        "Fighting", 
        19.99M,
        new DateOnly(1992, 7, 15)),
    new (        
        2, 
        "Final Fantasy VII Rebirth", 
        "RPG", 
        69.99M,
        new DateOnly(2024, 2, 29)),
    new (        
        3, 
        "Astro Bot", 
        "Platformer", 
        59.99M,
        new DateOnly(2024, 9, 6)),

];

// GET /games
app.MapGet("/games", () => games);



// GET /games/id
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName);

// POST /games
app.MapPost("/games",(CreateGameDto newgame) =>
{
   GameDto game = new(
    games.Count +1,
    newgame.Name,
    newgame.Genre,
    newgame.Price,
    newgame.ReleaseDate
   ); 

   games.Add(game);
   
   return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
});

// PUT /games/id
app.MapPut("/games/{id}", (int id, UpdateGameDto updateGameDto) =>
{
    int index = games.FindIndex(game => game.Id == id);

    games[index] = new GameDto(
        id,
        updateGameDto.Name,
        updateGameDto.Genre,
        updateGameDto.Price,
        updateGameDto.ReleaseDate
    );

    return Results.NoContent();
});

// DELETE /games/id
app.MapDelete("/games/{id}", (int id) =>
{
    games.RemoveAll(game => game.Id == id);

    return Results.NoContent();
});

app.Run();

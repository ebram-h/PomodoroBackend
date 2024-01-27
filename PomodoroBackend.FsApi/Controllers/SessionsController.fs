namespace PomodoroBackend.FsApi.Controllers

open Microsoft.AspNetCore.Mvc
open MongoDB.Driver
open PomodoroBackend.FsApi.Models

[<ApiController>]
[<Route("[controller]")>]
type SessionsController() =
    inherit ControllerBase()
    let connectionString = "mongodb://localhost:27017"
    let client = MongoClient(connectionString)
    let sessionsCollection = client.GetDatabase("Pomodoro").GetCollection("Sessions")
    
    [<HttpGet>]
    member _.getSessions() =
        let result = sessionsCollection.Find(fun d -> true)
        result.ToList()
    
    [<HttpPost>]
    member _.addSession(session: Session) =
        sessionsCollection.InsertOne(session)

        OkResult()
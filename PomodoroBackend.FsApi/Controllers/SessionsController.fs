namespace PomodoroBackend.FsApi.Controllers

open System
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
    member _.getSessions([<FromQuery>] date: string) =
        if String.IsNullOrWhiteSpace(date) then
            sessionsCollection.FindSync(Builders<Session>.Filter.Empty).ToList<Session>()
        else
            
        let mutable dateAtTheStartOfDay = DateTime.Now.Date

        if date.ToUpperInvariant() <> "TODAY" then
            let timestamp = Int64.Parse(date)
            dateAtTheStartOfDay <- DateTime.FromBinary(timestamp).Date

        let filterDaysBefore =
           Builders<Session>.Filter.Gte((fun s -> s.StartTime), dateAtTheStartOfDay.Ticks)

        let filterDaysAfter =
           Builders<Session>.Filter
             .Lt((fun s -> s.StartTime), dateAtTheStartOfDay.AddDays(1).Ticks)

        let aggregateFilter =
           Builders<Session>.Filter.And(filterDaysBefore, filterDaysAfter)

        sessionsCollection.FindSync(aggregateFilter).ToList()


    [<HttpPost>]
    member _.addSession(session: Session) =
        sessionsCollection.InsertOne(session)

        OkResult()

    [<HttpDelete>]
    member _.deleteSession(startTime: obj) =
        let filter =
            Builders<Session>.Filter.Eq((fun x -> x.StartTime), startTime :?> int64)

        sessionsCollection.FindOneAndDelete<Session> filter

namespace PomodoroBackend.FsApi.Models

open System
open System.Collections.Generic
open Newtonsoft.Json

type SpentTimeDetails =
    { Index: int
      NormalizedWeight: double
      Occurrence: int }

type AppDetailsInSession =
    { AppName: string
      SpentTimeInHour: double
      TitleSpentTime: IDictionary<string, SpentTimeDetails> }

type Session =
    { [<JsonProperty("_id")>] Id : string
      SwitchActivities: int array
      StayTimeInSecond: double array
      Apps: IDictionary<string, AppDetailsInSession>
      SpentTimeInHour: double
      SwitchTimes: int
      StartTime: int64
      Efficiency: double
      BoardId: string }
namespace HappyBever
open System
open System.Linq

[<AutoOpen>]
module StringExtensions =
    let isNullOrEmpty =
        fun (s : string) -> 
            String.IsNullOrEmpty(s)

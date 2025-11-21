module Approaches.Functional

type Point = { X: float; Y: float }

type Shape =
    | Circle of radius: float
    | Square of length: float
    | Translated of shape: Shape * offset: Point

let rec containsPoint shape point =
    match shape with
    | Circle radius -> point.X * point.X + point.Y * point.Y <= radius * radius
    | Square length -> point.X >= 0 && point.X <= length && point.Y >= 0 && point.Y <= length
    | Translated(shape, offset) ->
        let translatedPoint =
            { X = point.X - offset.X
              Y = point.Y - offset.Y }
        containsPoint shape translatedPoint

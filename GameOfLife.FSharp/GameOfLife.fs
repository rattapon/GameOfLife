namespace GameOfLife.FSharp

open System
    
    module GameOfLife = 
    
        let initialize role column = 
            if role < 1 || column < 1 then raise (System.ArgumentException("Cannot initialize with size that is smaller than 1"))
            let grid = Array2D.init role column (fun _ _ -> false)
            grid

        let wakeUpCell coordinates (grid:bool[,]) = 
            let mutable newGrid :bool[,] = Array2D.copy grid
            for coordinate in coordinates do
                match coordinate with 
                    | (x, y) -> newGrid.[x, y] <- true
            newGrid

        let isCellAlive x y (grid:bool[,]) = 
            if x < 0 || x >= Array2D.length1 grid ||
               y < 0 || y >= Array2D.length2 grid then
               0
            else
               if grid.[x, y] = true then 1 else 0

            

        let countNeightbours x y (grid:bool[,]) = 
            let topLeft = isCellAlive (x-1) (y+1) grid
            let topMid = isCellAlive x (y+1) grid
            let topRight = isCellAlive (x+1) (y+1) grid
            let left = isCellAlive (x-1) y grid
            let right = isCellAlive (x+1) y grid
            let bottomLeft = isCellAlive (x-1) (y-1) grid
            let bottomMid = isCellAlive x (y-1) grid
            let bottomRight = isCellAlive (x+1) (y-1) grid
            topLeft + topMid + topRight + left + right + bottomLeft + bottomMid + bottomRight
  

        [<CompiledName("Initialize")>]
        let init role column = initialize role column

        [<CompiledName("LiveCell")>]
        let wake coordinates grid = wakeUpCell coordinates grid

        [<CompiledName("CountNeighbours")>]
        let count x y grid = countNeightbours x y grid

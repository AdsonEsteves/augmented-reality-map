using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class globalMap {

    public static int[,] Mapa { get; set; }

    public static int[,] MapaDirecao { get; set; }

    public const int X_POSITIVO = 1;
    public const int X_NEGATIVO = 2;

    public const int Z_POSITIVO = 3;
    public const int Z_NEGATIVO = 4;
}

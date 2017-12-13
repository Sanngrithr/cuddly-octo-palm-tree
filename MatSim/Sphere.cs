using Fusee.Math.Core;

public class Sphere
{
    private float coordinateX(int t, int p){

        float vecx = (M.Cos(t) * M.Sin(p));

        return vecx;
    }

    private float coordinateY(int t, int p){

        float vecy =(M.Cos(t) * M.Sin(p));

        return vecy;
    }

    private float coordinateZ(int t, int p){

        float vecz = (M.Sin(t));

        return vecz;
    }

}
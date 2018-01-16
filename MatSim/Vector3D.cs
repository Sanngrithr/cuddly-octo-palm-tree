using Fusee.Math.Core;
using System;
public class Vector3D{

    public float x;
    public float y;
    public float z;

    public Vector3D(float x, float y, float z){

        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3D(float t, float p){

        this.x = M.Cos(t) * M.Sin(p);
        this.y = M.Cos(t) * M.Sin(p);
        this.z = M.Sin(t);
    }

    public float length(){
        var x = new float3(this.x, this.x, this.z);
        return x.Length;
    }

    public void normalize(){

        this.x = x/this.length();
        this.y = y/this.length();
        this.z = z/this.length();

        return;
    }

    public float dotproduct(Vector3D v){

        return (this.x * v.x + this.y * v.y + this.z * this.z);
    }

    public Vector3D crossproduct(Vector3D v){

        Vector3D uxv = new Vector3D(0,0,0);

        uxv.x = (this.y * v.z) - (this.z * v.y);
        uxv.y = (this.z * v.x) - (this.z * v.x);
        uxv.z = (this.x * v.y) - (this.y * v.x);

        return uxv;
    }

    public float angle(Vector3D v){
    var x = System.Math.Acos(Convert.ToDouble(this.dotproduct(v)/(this.length() * v.length())));
    return Convert.ToSingle(x);
    }

    public void rotation(float g, Vector3D drehachse){

        this.x = (
                  ((drehachse.x * drehachse.x) + M.Cos(g) * (1 - (drehachse.x * drehachse.x))) +
                  (drehachse.x * drehachse.y * (1 - M.Cos(g)) + drehachse.z * M.Sin(g)) +
                  (drehachse.x * drehachse.z * (1 - M.Cos(g)) - drehachse.y * M.Sin(g))
                 );

        this.y = (
                  (drehachse.x * drehachse.y * (1 - M.Cos(g)) - drehachse.z * M.Sin(g)) +
                  ((drehachse.y * drehachse.y) + M.Cos(g) * (1 - (drehachse.y * drehachse.y))) +
                  (drehachse.y * drehachse.z * (1 - M.Cos(g)) + drehachse.x * M.Sin(g))

                 );
        this.z = (
                  (drehachse.x * drehachse.z * (1 - M.Cos(g)) + drehachse.y * M.Sin(g)) +
                  (drehachse.y * drehachse.z * (1 - M.Cos(g)) - drehachse.x * M.Sin(g)) +
                  ((drehachse.z * drehachse.z) + M.Cos(g) * (1 - (drehachse.z * drehachse.z)))
                 );

        return;
    }
}
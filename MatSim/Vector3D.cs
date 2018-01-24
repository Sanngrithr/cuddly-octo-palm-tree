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

        this.z = -1*(M.Cos(t) * M.Cos(p));
        this.x = M.Cos(t) * M.Sin(p);
        this.y = M.Sin(t);
    }

    public float length(){
        var x = new float3(this.x, this.y, this.z);
        return x.Length;
    }

    public void normalize(){

        this.x = x/this.length();
        this.y = y/this.length();
        this.z = z/this.length();

        return;
    }

    public float dotproduct(Vector3D v){

        return (this.x * v.x + this.y * v.y + this.z * v.z);
    }

    public float3 crossproduct(float3 v, float3 u){

        float3 uxv = new float3(0,0,0);

        uxv.x = (u.y * v.z) - (u.z * v.y);
        uxv.y = (u.z * v.x) - (u.z * v.x);
        uxv.z = (u.x * v.y) - (u.y * v.x);

        return uxv;
    }

    public float angleRad(Vector3D v){
    var x = (float)Math.Acos(((double)this.dotproduct(v)/((double)this.length() * (double)v.length())));
    return x;
    }

    public void rotatate(float g, float3 drehachse){

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
using Fusee.Math.Core;
using System;
public class Vector3D{
    
    public double x;
    public double y;
    public double z;

    public Vector3D(double x, double y, double z){

        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Vector3D(double t, double p){

        this.x = System.Math.Cos(t) * System.Math.Sin(p);
        this.y = System.Math.Cos(t) * System.Math.Sin(p);
        this.z = System.Math.Sin(t);
    }

    public double length(){

        return System.Math.Sqrt(x*x + y*y + z*z);
    }

    public void normalize(){

        this.x = x/this.length();
        this.y = y/this.length();
        this.z = z/this.length();

        return;
    }

}
using System;
using Fusee.Math.Core;

public class Rotations
{
    public void xRotation(float3 vector, float angle){
        
        var xRotTopRow = new float3(1, 0, 0);
        var xRotMidRow = new float3(0, M.Cos(angle), -(M.Sin(angle)));
        var xRotBotRow = new float3(0, M.Sin(angle), M.Cos(angle));

        vector.x = xRotTopRow.x * vector.x + xRotMidRow.x * vector.x + xRotBotRow.x * vector.x;
        vector.y = xRotTopRow.y * vector.y + xRotMidRow.y * vector.y + xRotBotRow.y * vector.y;
        vector.z = xRotTopRow.z * vector.z + xRotMidRow.z * vector.z + xRotBotRow.z * vector.z;

    }
    
    public void yRotation(float3 vector, float angle){
        
        var yRotTopRow = new float3(M.Cos(angle), 0, M.Sin(angle));
        var yRotMidRow = new float3(0, 1, 0);
        var yRotBotRow = new float3(-(M.Sin(angle)), 0, M.Cos(angle));

        vector.x = yRotTopRow.x * vector.x + yRotMidRow.x * vector.x + yRotBotRow.x * vector.x;
        vector.y = yRotTopRow.y * vector.y + yRotMidRow.y * vector.y + yRotBotRow.y * vector.y;
        vector.z = yRotTopRow.z * vector.z + yRotMidRow.z * vector.z + yRotBotRow.z * vector.z;

    }

    public void zRotation(float3 vector, float angle){
        
        var zRotTopRow = new float3(M.Cos(angle), -(M.Sin(angle)), 0);
        var zRotMidRow = new float3(M.Sin(angle), M.Cos(angle), 0);
        var zRotBotRow = new float3(0, 0, 1);

        vector.x = zRotTopRow.x * vector.x + zRotMidRow.x * vector.x + zRotBotRow.x * vector.x;
        vector.y = zRotTopRow.y * vector.y + zRotMidRow.y * vector.y + zRotBotRow.y * vector.y;
        vector.z = zRotTopRow.z * vector.z + zRotMidRow.z * vector.z + zRotBotRow.z * vector.z;

    }
}
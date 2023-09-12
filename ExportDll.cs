using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using OpenHardwareMonitor.Hardware;


namespace OpenHardwareMonitor {

  public class UpdateVisitor : IVisitor {
    public void VisitComputer(IComputer computer) {
      computer.Traverse(this);
    }

    public void VisitHardware(IHardware hardware) {
      hardware.Update();
      foreach (IHardware subHardware in hardware.SubHardware)
        subHardware.Accept(this);
    }

    public void VisitSensor(ISensor sensor) { }

    public void VisitParameter(IParameter parameter) { }
  }

  static public class ExportDll {
    static private Computer computer;

    static ExportDll() {
      //Console.WriteLine("생성자 호출");

      computer = new Computer();
      computer.MainboardEnabled = true;
      computer.CPUEnabled = true;
      computer.RAMEnabled = true;
      computer.GPUEnabled = true;
      computer.FanControllerEnabled = true;
      //computer.HDDEnabled = true; 
    }

    [DllExport("prepareComputer", CallingConvention = CallingConvention.StdCall)]
    public static void prepareComputer() {
      //if (computer != null) return;
      //Console.WriteLine("컴퓨터 준비");
      computer.Open();

    }


    private static int CompareSensor(ISensor a, ISensor b) {
      int c = a.SensorType.CompareTo(b.SensorType);
      if (c == 0)
        return a.Index.CompareTo(b.Index);
      else
        return c;
    }

    private static string GenerateJSON(IHardware hardware) {
      string JSON = "{\"name\": \"" + hardware.Name + "\", ";
      JSON += "\"type\": " + (int)hardware.HardwareType + ", ";
      JSON += "\"type_str\": \"" + hardware.HardwareType.ToString() + "\", ";

      JSON += "\"identifier:\": \"" + hardware.Identifier + "\", ";

      ISensor[] sensors = hardware.Sensors;
      if (sensors.Length > 0) {
        JSON += "\"sensors\": [";

        Array.Sort(sensors, CompareSensor);

        foreach (ISensor sensor in sensors) {
          JSON += GenerateJSON(sensor);
          JSON += ", ";
        }

        if (JSON.EndsWith(", ")) JSON = JSON.Remove(JSON.LastIndexOf(","));
        JSON += "], ";
      }

      if (JSON.EndsWith(", ")) JSON = JSON.Remove(JSON.LastIndexOf(","));
      JSON += "}";

      return JSON;

    }

    private static string GenerateJSON(ISensor sensor) {
      string JSON = "{\"name\": \"" + sensor.Name + "\", ";
      JSON += "\"type\": \"" + (int)sensor.SensorType + "\", ";
      JSON += "\"type_str\": \"" + sensor.SensorType.ToString() + "\", ";

      if (sensor.Value.HasValue) {
        JSON += string.Format("\"value\": \"{0}\", ", sensor.Value);
        JSON += string.Format("\"min\": \"{0}\", ", sensor.Min);
        JSON += string.Format("\"max\": \"{0}\", ", sensor.Max);
      }
      /*
      JSON += "\"value\": \"" + sensor.Value.ToString() + "\", ";
      JSON += "\"min\": \"" + sensor.Min.ToString() + "\", ";
      JSON += "\"max\": \"" + sensor.Max.ToString() + "\", ";
      */

      JSON += "\"identifier:\": \"" + sensor.Identifier + "\"";
      JSON += "}";


      //if (sensor.IsDefaultHidden) JSON += "  Hidden";

      return JSON;
    }

    [DllExport("getComputerInfo", CallingConvention = CallingConvention.StdCall)]
    public static IntPtr getComputerInfo() {
      computer.Accept(new UpdateVisitor());

      string JSON = "{\"name\": \"" + System.Environment.MachineName + "\", ";

      JSON += "\"hardwares\": [";

      foreach (IHardware hardware in computer.Hardware) {
        JSON += GenerateJSON(hardware);
        JSON += ", ";
      }

      if (JSON.EndsWith(", ")) JSON = JSON.Remove(JSON.LastIndexOf(","));

      JSON += "]";

      JSON += "}";

      return Marshal.StringToBSTR(JSON);
    }

    /*
    [DllExport("testComputer", CallingConvention = CallingConvention.StdCall)]
    public static void testComputer() {
      computer.Accept(new UpdateVisitor());

      string r = Ring0.GetReport();
      if (r != null) {
        Console.WriteLine(r);
      }

      //Console.WriteLine(computer.GetReport());

      foreach (IHardware hardware in computer.Hardware) {
        //Console.WriteLine(hardware.Name + " " + hardware.Identifier);
        Console.WriteLine(GenerateJSON(hardware));
      }

    }
    */

    [DllExport("coloseComputer", CallingConvention = CallingConvention.StdCall)]
    public static void coloseComputer() {
      //Console.WriteLine("컴퓨터 정리");
      computer.Close();
    }


    /*
    [DllExport("test", CallingConvention = CallingConvention.StdCall)]
    public static IntPtr test(IntPtr input, ref IntPtr input2) {
      string str_input = Marshal.PtrToStringUni(input);
      Console.WriteLine("input: " + str_input);

      string str_input2 = "테스트야 테스트";
      input2 = Marshal.StringToBSTR(str_input2); 

      string r = "울랄라!";
      return Marshal.StringToBSTR(r); 
    }
    */


    

  }
}

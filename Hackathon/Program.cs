using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siemens.Simatic.Simulation.Runtime;
using Hackathon.Interfaces;

namespace Hackathon
{
    class Program
{
        static IApiCaller Caller;
        static void Main(string[]args)
{ 
            Caller = new ApiCaller();
            Caller.SetVariable("IX_SSC_LightBarrierOutsourcing_I4", true);
            Caller.SetVariable("QX_SLD_ValveThirdEjectorBlue_Q5", false);
            Caller.SetVariable("QX_MPO_M4_OvenFeederRetract_Q5", false);
            Caller.SetVariable("QW_MPO_PWM_TurnTable_M1", 1000);
            Caller.SetVariable("IX_MPO_RefSwitchOvenFeederOutside_I7", false);
            Caller.SetVariable("QX_VGR_M3_RotateClockwise_Q5", false);
            Caller.SetVariable("QW_VGR_PWM_Horizontal_M2", 500);
            Caller.SetVariable("IX_MPO_RefSwitchVac_PosOven_I8", false);
            Caller.SetVariable("QX_VGR_M3_RotateCounterclockwise_Q6", false);
            Caller.SetVariable("QW_HBW_PWM_HorizontalAxis_M2", 700);
            Caller.SetVariable("QW_HBW_PWM_ConveyorBelt_M1", 800);
            Caller.SetVariable("IX_MPO_RefSwitchVac_PosTurnTable_I5", true);
            Caller.SetVariable("IX_VGR_EncoderRotateImp2_B6", false);
            Caller.SetVariable("QX_MPO_M1_TurnTableClockwise_Q1", false);
            Caller.SetVariable("IW_SSC_ColorSensor_A1", 23292);
            Caller.SetVariable("IX_HBW_EncoderVerticalAxisImp2_B4", true);
            Caller.SetVariable("IX_HBW_TrailSensor1Lower_A1", false);
            Caller.SetVariable("IX_HBW_SwitchCantileverBack_I6", true);
            Caller.SetVariable("QX_VGR_ValveVacuum_Q8", false);
            Caller.SetVariable("QX_SLD_ValveSecondEjectorRed_Q4", false);
            Caller.SetVariable("IX_MPO_LightBarrierOven_I9", true);
            Caller.SetVariable("IX_MPO_LightBarrierEndOfConBelt_I3", true);
            Caller.SetVariable("IX_HBW_EncoderVerticalAxisImp1_B3", false);
            Caller.SetVariable("QX_SSC_M2_HorizontalAxisClockwise_Q3", false);
            Caller.SetVariable("IX_SLD_LightBarrierWhite_I5", true);
            Caller.SetVariable("QX_MPO_M5_VacuumTowardsOven_Q7", false);
            Caller.SetVariable("QX_MPO_M2_ConveyorBeltForward_Q3", false);
            Caller.SetVariable("IX_HBW_TrailSensor2Upper_A2", false);
            Caller.SetVariable("IX_VGR_RefSwitchRotate_I3", true);
            Caller.SetVariable("IX_SSC_EncoderVerticalAxisImp1_B1", false);
            Caller.SetVariable("QX_VGR_M1_VerticalAxisDown_Q2", false);
            Caller.SetVariable("QX_MPO_M3_Saw_Q4", false);
            Caller.SetVariable("IX_SLD_LightBarrierRed_I6", true);
            Caller.SetVariable("QW_VGR_PWM_Rotate_M3", 500);
            Caller.SetVariable("QW_MPO_PWM_Vacuum_M5", 1000);
            Caller.SetVariable("IX_SSC_EncoderHorizontalAxisImp2_B4", false);
            Caller.SetVariable("IX_Set_Park_Position", false);
            Caller.SetVariable("QX_MPO_Compressor_Q10", false);
            Caller.SetVariable("QW_SSC_PWM_Vertical_M1", 1000);
            Caller.SetVariable("IX_SSC_RefSwitchHorizontalAxis_I2", false);
            Caller.SetVariable("IX_HBW_LightBarrierInside_I2", true);
            Caller.SetVariable("IX_VGR_EncoderHorizontalAxisImp1_B3", true);
            Caller.SetVariable("QW_HBW_PWM_Cantilever_M4", 1000);
            Caller.SetVariable("QX_SSC_M1_VerticalAxisUp_Q1", false);
            Caller.SetVariable("IX_MPO_RefSwitchTurnTable_PosSaw_I4", false);
            Caller.SetVariable("IX_HBW_LightBarrierOutside_I3", true);
            Caller.SetVariable("IX_VGR_RefSwitchHorizontalAxis_I2", true);
            Caller.SetVariable("QX_HBW_M3_VerticalAxisDownward_Q5", false);
            Caller.SetVariable("QX_SSC_LED_Red_Q7", false);
            Caller.SetVariable("QX_HBW_M4_CantileverBackward_Q8", false);
            Caller.SetVariable("IX_HBW_RefSwitchVerticalAxis_I4", true);
            Caller.SetVariable("QX_HBW_M1_ConveyorBeltBackward_Q2", false);
            Caller.SetVariable("QX_MPO_ValveOvenDoor_Q13", false);
            Caller.SetVariable("QX_VGR_M1_VerticalAxisUp_Q1", false);
            Caller.SetVariable("IX_MPO_RefSwitchTurnTable_PosBelt_I2", false);
            Caller.SetVariable("QX_SLD_Compressor_Q2", false);
            Caller.SetVariable("IX_HBW_RefSwitchHorizontalAxis_I1", true);
            Caller.SetVariable("IX_HBW_SwitchCantileverFront_I5", false);
            Caller.SetVariable("IX_SLD_LightBarrierBlue_I7", true);
            Caller.SetVariable("IX_VGR_EncoderVerticalAxisImp1_B1", false);
            Caller.SetVariable("IX_VGR_EncoderVerticalAxisImp2_B2", false);
            Caller.SetVariable("IX_HBW_EncoderHorizontalAxisImp2_B2", true);
            Caller.SetVariable("QX_VGR_Compressor_Q7", false);
            Caller.SetVariable("QX_SSC_LED_Green_Q5", true);
            Caller.SetVariable("QW_HBW_PWM_VerticalAxis_M3", 600);
            Caller.SetVariable("IX_SSC_EncoderHorizontalAxisImp1_B3", true);
            Caller.SetVariable("QX_VGR_M2_HorizontalAxisBackward_Q3", false);
            Caller.SetVariable("QX_SLD_M1_ConveyorBelt_Q1", false);
            Caller.SetVariable("QX_MPO_M1_TurnTableCounterclockwise_Q2", false);
            Caller.SetVariable("IX_VGR_EncoderRotateImp1_B5", false);
            Caller.SetVariable("IX_MPO_RefSwitchTurnTable_PosVac_I1", true);
            Caller.SetVariable("IX_Fill_HBW", false);
            Caller.SetVariable("QX_MPO_ValveFeeder_Q14", false);
            Caller.SetVariable("QX_SLD_ValveFirstEjectorWhite_Q3", false);
            Caller.SetVariable("IX_SSC_RefSwitchVerticalAxis_I1", false);
            Caller.SetVariable("IX_SLD_PulseCounter_I1", false);
            Caller.SetVariable("QX_MPO_M5_VacuumTowardsTurnTable_Q8", false);
            Caller.SetVariable("IW_SLD_ColorSensor_A4", 18720);
            Caller.SetVariable("QX_SSC_M2_HorizontalAxisCounterclockwise_Q4", false);
            Caller.SetVariable("QW_SSC_PWM_Horizontal_M2", 1000);
            Caller.SetVariable("QX_MPO_M4_OvenFeederExtend_Q6", false);
            Caller.SetVariable("QX_MPO_ValveLowering_Q12", false);
            Caller.SetVariable("QX_MPO_ValveVacuum_Q11", false);
            Caller.SetVariable("QX_HBW_M4_CantileverForward_Q7", false);
            Caller.SetVariable("QX_SSC_LED_Red_Online_Q8", true);
            Caller.SetVariable("IX_VGR_EncoderHorizontalAxisImp2_B4", false);
            Caller.SetVariable("IX_VGR_RefSwitchVerticalAxis_I1", true);
            Caller.SetVariable("QX_SSC_M1_VerticalAxisDown_Q2", false);
            Caller.SetVariable("QX_VGR_M2_HorizontalAxisForward_Q4", false);
            Caller.SetVariable("QX_HBW_M2_HorizontalTowardsConveyorBelt_Q4", false);
            Caller.SetVariable("IX_MPO_RefSwitchOvenFeederInside_I6", true);
            Caller.SetVariable("QX_HBW_M1_ConveyorBeltForward_Q1", false);
            Caller.SetVariable("IW_SSC_ColorSensor_A1", 23291);
            Caller.SetVariable("IW_SLD_ColorSensor_A4", 18719);
            Caller.SetVariable("QX_SSC_LED_Red_Online_Q8", false);
            Caller.SetVariable("QX_HBW_M2_HorizontalTowardsRack_Q3", false);
            Caller.SetVariable("QX_HBW_M3_VerticalAxisUpward_Q6", false);
            Caller.SetVariable("IX_HBW_EncoderHorizontalAxisImp1_B1", true);
            Caller.SetVariable("QX_SSC_LED_Yellow_Q6", false);
            Caller.SetVariable("QX_MPO_LightOven_Q9", false);
            Caller.SetVariable("IX_SSC_EncoderVerticalAxisImp2_B2", false);
            Caller.SetVariable("IX_SLD_LightBarrierBehindColorSensor_I3", true);
            Caller.SetVariable("IX_SLD_LightBarrierInlet_I2", true);
            Caller.SetVariable("QW_VGR_PWM_Vertical_M1", 800);
            Caller.SetVariable("IX_SSC_LightBarrierStorage_I3", true);

            Caller.RunToNextSyncPoint();

            /*
            Caller.SetVariable("IX_SSC_LightBarrierStorage_I3", false);
            Caller.RunToNextSyncPoint();
            Console.WriteLine(Caller.CheckVariable("QX_VGR_M3_RotateClockwise_Q5", true));
            */

            Console.ReadKey();
        }
    }
}

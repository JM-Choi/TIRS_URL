using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using TechFloor.Shared;

namespace TechFloor.Vision
{
  partial class CompositeImageProcessControlOld
  {
    ///////////////////////// START WIZARD GENERATED
    // cognex.wizard.globals.begin
    private const string mVppFilename = @"Cognex\TIRS_REV1_ASM.vpp";
    private const string mApplicationName = "TIRS_ASM_REV1";
    private static bool mUsePasswords = false;
    // private static bool mQuickBuildAccess = true;
    private const string mDefaultAdministratorPassword = "";
    private const string mDefaultSupervisorPassword = "";
    private static DateTime mGenerationDateTime = new DateTime(2020,8,19,5,15,34);
    private static string mGeneratedByVersion = "63.1.2.0";
    // cognex.wizard.globals.end
    ///////////////////////// END WIZARD GENERATED

    private void Wizard_FormLoad()
    {
      ///////////////////////// START WIZARD GENERATED
      // begin cognex.wizard.formloadactions
      cogToolPropertyProvider0.ErrorProvider = applicationErrorProvider;
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, checkBox_Job0_USEFOUNDCIRCLEVERIFY, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"UsingVerify\"].(System.Boolean)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, checkBox_Job0_USEREELCENTERPATTERN, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"UsingReelCenterPattern\"].(System.Boolean)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_PROCESS, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"ProcessType\"].(System.Int32)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_RMSERROR, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"RMSErrorLimit\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_SCORELIMIT, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"AcceptedPatternMathScore\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_LIMITX, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"CenterXOffsetLimit\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_LIMITY, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"CenterYOffsetLimit\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_TARGETPOINT, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"TargetPointRadius\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_REEL7, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"Reel7Radius\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_REEL13, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"Reel13Radius\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_BASEPOINTMODE, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"FindBasePointMode\"].(System.Int32)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_GUIDEPOINTMODE, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"FindGuidePointMode\"].(System.Int32)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_REFERENCEX, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"ReferenceOffsetX\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_REFERENCEY, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"ReferenceOffsetY\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_PITCHX, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"GuidePitchX\"].(System.Double)");
      Utility.SetupPropertyProvider(cogToolPropertyProvider0, textBox_Job0_PITCHY, mJM.Job(0).VisionTool, "UserData.Item[\"ScriptData\"].(Cognex.VisionPro.CogDictionary).Item[\"GuidePitchY\"].(System.Double)");
      // end cognex.wizard.formloadactions
      ///////////////////////// END WIZARD GENERATED
    }

    private void Wizard_AttachPropertyProviders()
    {
      ///////////////////////// START WIZARD GENERATED
      // begin cognex.wizard.attachpropertyproviders
      cogToolPropertyProvider0.Subject = mJM.Job(0).VisionTool;
      // end cognex.wizard.attachpropertyproviders
      ///////////////////////// END WIZARD GENERATED
    }

    private void Wizard_DetachPropertyProviders()
    {
      ///////////////////////// START WIZARD GENERATED
      // begin cognex.wizard.detachpropertyproviders
      cogToolPropertyProvider0.Subject = null;
      // end cognex.wizard.detachpropertyproviders
      ///////////////////////// END WIZARD GENERATED
    }

        private void Wizard_EnableControls(bool running)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => { SetControlsEnable(running); }));
            }
            else
                SetControlsEnable(running);
        }

        private void SetControlsEnable(bool running)
        {
            ///////////////////////// START WIZARD GENERATED
            // begin cognex.wizard.enablecontrols
            checkBox_Job0_USEFOUNDCIRCLEVERIFY.Enabled = ! running;
            checkBox_Job0_USEREELCENTERPATTERN.Enabled = ! running;
            textBox_Job0_PROCESS.Enabled = ! running;
            textBox_Job0_RMSERROR.Enabled = ! running;
            textBox_Job0_SCORELIMIT.Enabled = ! running;
            textBox_Job0_LIMITX.Enabled = ! running;
            textBox_Job0_LIMITY.Enabled = ! running;
            textBox_Job0_TARGETPOINT.Enabled = ! running;
            textBox_Job0_REEL7.Enabled = ! running;
            textBox_Job0_REEL13.Enabled = ! running;
            textBox_Job0_BASEPOINTMODE.Enabled = ! running;
            textBox_Job0_GUIDEPOINTMODE.Enabled = !running;
            textBox_Job0_REFERENCEX.Enabled = ! running;
            textBox_Job0_REFERENCEY.Enabled = ! running;
            textBox_Job0_PITCHX.Enabled = ! running;
            textBox_Job0_PITCHY.Enabled = ! running;
            // end cognex.wizard.enablecontrols
            ///////////////////////// END WIZARD GENERATED
        }

        private void Wizard_AddJobTabs(System.Collections.ArrayList newPagesList)
        {
            ///////////////////////// START WIZARD GENERATED
            // begin cognex.wizard.addjobtabs
            switch (mSelectedJob)
            {
                case 0:
                  newPagesList.Add(tabPage_Job0_URL);
                  newPagesList.Add(tabPage_Job0_BASEPOINT);
                  newPagesList.Add(tabPage_Job0_GUIDEPOINT);
                  break;
            }
            // end cognex.wizard.addjobtabs
            ///////////////////////// END WIZARD GENERATED
        }

        private void Wizard_UpdateJobResults(int idx, Cognex.VisionPro.ICogRecord result)
        {
            ///////////////////////// START WIZARD GENERATED
            // begin cognex.wizard.updatejobresults
            switch (idx)
            {
                case 0:
                    {
                        Utility.FillUserResultData(textBox_Job0_CODE, result, "ResultCode", false);
                        Utility.FillUserResultData(textBox_Job0_STATUS, result, "RunStatusResult", false);
                        Utility.FillUserResultData(textBox_Job0_PSTIME, result, "RunStatusProcessingTime", false);
                        Utility.FillUserResultData(textBox_Job0_TOTALTIME, result, "RunStatusTotalTime", false);
                        Utility.FillUserResultData(textBox_Job0_SLOTEMPTY, result, "ResultEmpty", false);
                        Utility.FillUserResultData(textBox_Job0_CARTTYPE, result, "ResultCartType", false);
                        Utility.FillUserResultData(textBox_Job0_RADIUS, result, "ResultRadius", false);
                        Utility.FillUserResultData(textBox_Job0_CENTERX, result, "ResultCenterX", false);
                        Utility.FillUserResultData(textBox_Job0_CENTERY, result, "ResultCenterY", false);
                        Utility.FillUserResultData(textBox_Job0_SCORE, result, "ResultMatchScore", false);
                        Utility.FillUserResultData(textBox_Job0_SCALING, result, "ResultScaling", false);
                        Utility.FillUserResultData(textBox_Job0_RMSERRORVALUE, result, "ResultRMSError", false);
                        Utility.FillUserResultData(textBox_Job0_QRCOUNT, result, "ResultQRCount", false);
                        Utility.FillUserResultData(textBox_Job0_BARCODEPPM, result, "ResultQRPPM", false);
                        Utility.FillUserResultData(textBox_Job0_REMAINED, result, "ResultRemainedParts", false);
                        Utility.FillUserResultData(textBox_Job0_REMAINEDPPM, result, "ResultRemainedPPM", false);
                        Utility.FillUserResultData(textBox_Job0_DECODEDATA, result, "ResultBarcodeData", false);
                        Utility.FillUserResultData(textBox_Job0_MESSAGE, result, "RunStatusMessage", false);
                        Utility.FillUserResultData(textBox_Job0_Base_VERIFIED, result, "ResultVerifiedBasePoint", false);
                        Utility.FillUserResultData(textBox_Job0_FOUND1st, result, "FirstBasePointFound", false);
                        Utility.FillUserResultData(textBox_Job0_FOUND2nd, result, "SecondBasePointFound", false);
                        Utility.FillUserResultData(textBox_Job0_Base_1stX, result, "FirstBasePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Base_1stY, result, "FirstBasePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Base_1stAngle, result, "FirstBasePointAngle", false);
                        Utility.FillUserResultData(textBox_Job0_Base_2ndX, result, "SecondBasePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Base_2ndY, result, "SecondBasePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Base_2ndAngle, result, "SecondBaePointAngle", false);
                        Utility.FillUserResultData(textBox_Job0_Base_DISX, result, "DisplacementBasePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Base_DISY, result, "DisplacementBasePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Base_DISAngle, result, "DisplacementBasePointAngle", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_VERIFIED, result, "ResultVerifiedGuidePoint", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_1stX, result, "FirstGuidePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_1stY, result, "FirstGuidePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_1stAngle, result, "FirstGuidePointAngle", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_2ndX, result, "SecondGuidePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_2ndY, result, "SecondGuidePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_2ndAngle, result, "SecondGuidePointAngle", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_3rdX, result, "ThirdGuidePointX", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_3rdY, result, "ThirdGuidePointY", false);
                        Utility.FillUserResultData(textBox_Job0_Guide_3rdAngle, result, "ThirdGuidePointAngle", false);
                        //Utility.FillUserResultData(textBox_Job0_Guide_4thX, result, "ForthGuidePointX", false);
                        //Utility.FillUserResultData(textBox_Job0_Guide_4thY, result, "ForthGuidePointY", false);
                        //Utility.FillUserResultData(textBox_Job0_Guide_4thAngle, result, "ForthGuidePointAngle", false);

                        try
                        {
                            // Need confirm
                            // Error = -1, Accept = 0, Warning = 1, Reject = 2
                            if (!string.IsNullOrEmpty(textBox_Job0_CODE.Text) && !textBox_Job0_CODE.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.ResultCode, (Cognex.VisionPro.CogToolResultConstants)Enum.Parse(typeof(Cognex.VisionPro.CogToolResultConstants), textBox_Job0_CODE.Text));
                            else
                                return;

                            if (!string.IsNullOrEmpty(textBox_Job0_STATUS.Text) && !textBox_Job0_STATUS.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.RunStatus, (Cognex.VisionPro.CogToolResultConstants)Enum.Parse(typeof(Cognex.VisionPro.CogToolResultConstants), textBox_Job0_STATUS.Text));

                            if (!string.IsNullOrEmpty(textBox_Job0_DECODEDATA.Text) && !textBox_Job0_DECODEDATA.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundQrCode, textBox_Job0_DECODEDATA.Text);
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundQrCode, string.Empty);

                            if (!string.IsNullOrEmpty(textBox_Job0_CARTTYPE.Text) && !textBox_Job0_CARTTYPE.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.UserData_0, int.Parse(textBox_Job0_CARTTYPE.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.UserData_0, 0);

                            if (!string.IsNullOrEmpty(textBox_Job0_SLOTEMPTY.Text) && !textBox_Job0_SLOTEMPTY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.UserData_1, bool.Parse(textBox_Job0_SLOTEMPTY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.UserData_1, false);

                            if (!string.IsNullOrEmpty(textBox_Job0_REMAINED.Text) && !textBox_Job0_REMAINED.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.UserData_2, int.Parse(textBox_Job0_REMAINED.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.UserData_2, -1);

                            if (!string.IsNullOrEmpty(textBox_Job0_QRCOUNT.Text) && !textBox_Job0_QRCOUNT.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.UserData_3, int.Parse(textBox_Job0_QRCOUNT.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.UserData_3, -1);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_VERIFIED.Text) && !textBox_Job0_Base_VERIFIED.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.VerifiedBasePoint, bool.Parse(textBox_Job0_Base_VERIFIED.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.VerifiedBasePoint, false);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_VERIFIED.Text) && !textBox_Job0_Guide_VERIFIED.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.VerifiedGuidePoint, bool.Parse(textBox_Job0_Guide_VERIFIED.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.VerifiedGuidePoint, false);

                            if (!string.IsNullOrEmpty(textBox_Job0_SCORE.Text) && !textBox_Job0_SCORE.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.MatchScore, double.Parse(textBox_Job0_SCORE.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.MatchScore, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_SCALING.Text) && !textBox_Job0_SCALING.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.MatchScale, double.Parse(textBox_Job0_SCALING.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.MatchScale, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_RMSERRORVALUE.Text) && !textBox_Job0_RMSERRORVALUE.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.RMSError, double.Parse(textBox_Job0_RMSERRORVALUE.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.RMSError, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_CENTERX.Text) && !textBox_Job0_CENTERX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundPatternCenterX, double.Parse(textBox_Job0_CENTERX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundPatternCenterY, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_CENTERY.Text) && !textBox_Job0_CENTERY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundPatternCenterY, double.Parse(textBox_Job0_CENTERY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundPatternCenterY, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_RADIUS.Text) && !textBox_Job0_RADIUS.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundPatternRadius, double.Parse(textBox_Job0_RADIUS.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundPatternRadius, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_1stX.Text) && !textBox_Job0_Guide_1stX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX1, double.Parse(textBox_Job0_Guide_1stX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_1stY.Text) && !textBox_Job0_Guide_1stY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY1, double.Parse(textBox_Job0_Guide_1stY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_1stAngle.Text) && !textBox_Job0_Guide_1stAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle1, double.Parse(textBox_Job0_Guide_1stAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_2ndX.Text) && !textBox_Job0_Guide_2ndX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX2, double.Parse(textBox_Job0_Guide_2ndX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_2ndY.Text) && !textBox_Job0_Guide_2ndY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY2, double.Parse(textBox_Job0_Guide_2ndY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_2ndAngle.Text) && !textBox_Job0_Guide_2ndAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle2, double.Parse(textBox_Job0_Guide_2ndAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_3rdX.Text) && !textBox_Job0_Guide_3rdX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX3, double.Parse(textBox_Job0_Guide_3rdX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX3, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_3rdY.Text) && !textBox_Job0_Guide_3rdY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY3, double.Parse(textBox_Job0_Guide_3rdY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY3, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Guide_3rdAngle.Text) && !textBox_Job0_Guide_3rdAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle3, double.Parse(textBox_Job0_Guide_3rdAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle3, -99999.9);

                            // if (!string.IsNullOrEmpty(textBox_Job0_4thX.Text) && !textBox_Job0_4thX.Text.Contains("<"))
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX4, double.Parse(textBox_Job0_4thX.Text));
                            // else
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointX4, -99999.9);

                            // if (!string.IsNullOrEmpty(textBox_Job0_4thY.Text) && !textBox_Job0_4thY.Text.Contains("<"))
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY4, double.Parse(textBox_Job0_4thY.Text));
                            // else
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointY4, -99999.9);

                            // if (!string.IsNullOrEmpty(textBox_Job0_4thAngle.Text) && !textBox_Job0_4thAngle.Text.Contains("<"))
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle4, double.Parse(textBox_Job0_4thAngle.Text));
                            // else
                            //     lastRunResult.SetValue(ResultDataElements.FoundCartGuidePointAngle4, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_1stX.Text) && !textBox_Job0_Base_1stX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointX1, double.Parse(textBox_Job0_Base_1stX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointX1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_1stY.Text) && !textBox_Job0_Base_1stY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointY1, double.Parse(textBox_Job0_Base_1stY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointY1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_1stAngle.Text) && !textBox_Job0_Base_1stAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointAngle1, double.Parse(textBox_Job0_Base_1stAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointAngle1, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_2ndX.Text) && !textBox_Job0_Base_2ndX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointX2, double.Parse(textBox_Job0_Base_2ndX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointX2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_2ndY.Text) && !textBox_Job0_Base_2ndY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointY2, double.Parse(textBox_Job0_Base_2ndY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointY2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_2ndAngle.Text) && !textBox_Job0_Base_2ndAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointAngle2, double.Parse(textBox_Job0_Base_2ndAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointAngle2, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_DISX.Text) && !textBox_Job0_Base_DISX.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementX, double.Parse(textBox_Job0_Base_DISX.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementX, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_DISY.Text) && !textBox_Job0_Base_DISY.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementY, double.Parse(textBox_Job0_Base_DISY.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementY, -99999.9);

                            if (!string.IsNullOrEmpty(textBox_Job0_Base_DISAngle.Text) && !textBox_Job0_Base_DISAngle.Text.Contains("<"))
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementAngle, double.Parse(textBox_Job0_Base_DISAngle.Text));
                            else
                                lastRunResult.SetValue(ResultDataElements.FoundTowerBasePointDisplacementAngle, -99999.9);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                            MessageBox.Show($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}: Exception={ex.Message}");
                        }
                    }
                    break;
          }
          // end cognex.wizard.updatejobresults
          ///////////////////////// END WIZARD GENERATED
        }

  }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XRayScan.Log
{
    public enum LogMode 
    {
        TestMode,
        ReadingMode,
    }

    public struct TrackData
    {
        public bool isLegal;
        public float trackTime;
        public Vector3 trackPosition;
    }

    public class XrayLog
    {
        protected DateTime logSystemTime;
        protected float logPlayTime;
        protected LogMode logMode;
        protected int hGrid;
        protected int vGrid;
        protected List<TrackData> trackDatas;
        protected Vector3 leftTopPosition;
        protected Vector3 rightTopPosition;
        protected Vector3 leftBottomPosition;
        protected Vector3 rightBottomPosition;

        public LogMode GetLogMode() { return this.logMode; }
        public virtual string LogDataToString() { return string.Empty; }

        public void AddCornerPosition(Vector3 ltPosition, Vector3 rtPosition, Vector3 lbPosition, Vector3 rbPosition)
        {
            this.leftTopPosition = ltPosition;
            this.rightTopPosition = rtPosition;
            this.leftBottomPosition = lbPosition;
            this.rightBottomPosition = rbPosition;
        }

        public void AddTrackData(bool isLegal, Vector3 trackPos)
        {
            TrackData data;
            data.trackTime = Time.time - this.logPlayTime;
            data.isLegal = isLegal;
            data.trackPosition = trackPos;
            this.trackDatas.Add(data);
        }
    }

    public class TestXrayLog : XrayLog
    {
        public enum TestModeResultType
        {
            Success,
            Interrupt,
            TimesUp,
        }

        private struct TestModeCompleteGrid
        {
            public int hIndex;
            public int vIndex;
            public float completeTime;
        }

        private List<TestModeCompleteGrid> completeGrids;
        private TestModeResultType testModeResult;
        

        public TestXrayLog() 
        {
            this.completeGrids = new List<TestModeCompleteGrid>();
            this.trackDatas = new List<TrackData>();
            this.logMode = LogMode.TestMode;
            this.logSystemTime = DateTime.Now;
            this.logPlayTime = Time.time;
            this.hGrid = Config.ConfigManager.Instance.Config.horizontalPartition;
            this.vGrid = Config.ConfigManager.Instance.Config.verticalPartition;
        }

        public void AddResult(TestModeResultType result)
        {
            this.testModeResult = result;
        }

        public void AddCompleteGrid(int hIndex, int vIndex)
        {
            TestModeCompleteGrid grid;
            grid.hIndex = hIndex;
            grid.vIndex = vIndex;
            grid.completeTime = Time.time - this.logPlayTime;
            completeGrids.Add(grid);
        }

        public override string LogDataToString()
        {
            string logResult = "\n";
            logResult += "#start time:" + this.logSystemTime + "\n";
            logResult += "#result:" + this.testModeResult.ToString() + "\n";
            logResult += "#mode:" + this.logMode.ToString() + "\n";
            logResult += "#grid:" + this.hGrid.ToString() + " * " + this.vGrid.ToString() + "\n";
            logResult += "#complete grids:" + "\n";
            for (int i = 0; i < this.completeGrids.Count; i++)
            {
                logResult += "-[" + (completeGrids[i].hIndex + 1).ToString() + ", " +
                    (completeGrids[i].vIndex + 1).ToString() + "]:" +
                    completeGrids[i].completeTime.ToString("#0.000") + "\n";
            }

            logResult += "#4 corners:" + "\n";
            logResult += "-Left Top Position:(" + this.leftTopPosition.x.ToString("#0.0000") +
                        "," + this.leftTopPosition.y.ToString("#0.0000") +
                        "," + this.leftTopPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Right Top Position:(" + this.rightTopPosition.x.ToString("#0.0000") +
                        "," + this.rightTopPosition.y.ToString("#0.0000") +
                        "," + this.rightTopPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Left Bottom Position:(" + this.leftBottomPosition.x.ToString("#0.0000") +
                        "," + this.leftBottomPosition.y.ToString("#0.0000") +
                        "," + this.leftBottomPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Right Bottom Position:(" + this.rightBottomPosition.x.ToString("#0.0000") +
                        "," + this.rightBottomPosition.y.ToString("#0.0000") +
                        "," + this.rightBottomPosition.z.ToString("#0.0000") + ")" + "\n";

            logResult += "#scan track:" + "\n";
            for (int i = 0; i < this.trackDatas.Count; i++)
            {
                logResult += "-Time:" + this.trackDatas[i].trackTime.ToString("#0.000");
                if (trackDatas[i].isLegal)
                {
                    logResult += ",-Position:(" + this.trackDatas[i].trackPosition.x.ToString("#0.0000") +
                        "," + this.trackDatas[i].trackPosition.y.ToString("#0.0000") +
                        "," + this.trackDatas[i].trackPosition.z.ToString("#0.0000") + ")" + "\n";
                }
                else
                {
                    logResult += ",-Position:NaN" + "\n";
                }
            }

            logResult += "#endlog";
            logResult += "\n";
            return logResult;
        }
    }

    public class ReadingXrayLog : XrayLog
    {
        public enum ReadingModeResultType
        {
            Success,
            Fail,
            Interrupt,
        }

        private struct ReadingModeCompleteGrid
        {
            public int hIndex;
            public int vIndex;
            public float completeTime;
        }

        private List<ReadingModeCompleteGrid> completeGridsBeforeChecked;
        private List<ReadingModeCompleteGrid> completeGridsAfterChecked;
        private ReadingModeResultType readingModeResult;

        public ReadingXrayLog()
        {
            this.completeGridsBeforeChecked = new List<ReadingModeCompleteGrid>();
            this.completeGridsAfterChecked = new List<ReadingModeCompleteGrid>();
            this.trackDatas = new List<TrackData>();
            this.logMode = LogMode.ReadingMode;
            this.logSystemTime = DateTime.Now;
            this.logPlayTime = Time.time;
            this.hGrid = Config.ConfigManager.Instance.Config.horizontalPartition;
            this.vGrid = Config.ConfigManager.Instance.Config.verticalPartition;
        }

        public void AddResult(ReadingModeResultType result)
        {
            this.readingModeResult = result;
        }

        public void AddCompleteGrid(int hIndex, int vIndex, bool afterInformChecked)
        {
            ReadingModeCompleteGrid grid;
            grid.hIndex = hIndex;
            grid.vIndex = vIndex;
            grid.completeTime = Time.time - this.logPlayTime;
            if (!afterInformChecked)
            {
                this.completeGridsBeforeChecked.Add(grid);
            }
            else 
            {
                this.completeGridsAfterChecked.Add(grid);
            }
        }

        public override string LogDataToString()
        {
            string logResult = "\n";
            logResult += "#start time:" + this.logSystemTime + "\n";
            logResult += "#result:" + this.readingModeResult.ToString() + "\n";
            logResult += "#mode:" + this.logMode.ToString() + "\n";
            logResult += "#grid:" + this.hGrid.ToString() + " * " + this.vGrid.ToString() + "\n";
            logResult += "#complete grids(Before Checked):" + "\n";
            for (int i = 0; i < this.completeGridsBeforeChecked.Count; i++)
            {
                logResult += "-[" + (completeGridsBeforeChecked[i].hIndex + 1).ToString() + ", " +
                    (completeGridsBeforeChecked[i].vIndex + 1).ToString() + "]:" +
                    completeGridsBeforeChecked[i].completeTime.ToString("#0.000") + "\n";
            }
            logResult += "#complete grids(After Checked):" + "\n";
            for (int i = 0; i < this.completeGridsAfterChecked.Count; i++)
            {
                logResult += "-[" + (completeGridsAfterChecked[i].hIndex + 1).ToString() + ", " +
                    (completeGridsAfterChecked[i].vIndex + 1).ToString() + "]:" +
                    completeGridsAfterChecked[i].completeTime.ToString("#0.000") + "\n";
            }

            logResult += "#4 corners:" + "\n";
            logResult += "-Left Top Position:(" + this.leftTopPosition.x.ToString("#0.0000") +
                        "," + this.leftTopPosition.y.ToString("#0.0000") +
                        "," + this.leftTopPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Right Top Position:(" + this.rightTopPosition.x.ToString("#0.0000") +
                        "," + this.rightTopPosition.y.ToString("#0.0000") +
                        "," + this.rightTopPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Left Bottom Position:(" + this.leftBottomPosition.x.ToString("#0.0000") +
                        "," + this.leftBottomPosition.y.ToString("#0.0000") +
                        "," + this.leftBottomPosition.z.ToString("#0.0000") + ")" + "\n";
            logResult += "-Right Bottom Position:(" + this.rightBottomPosition.x.ToString("#0.0000") +
                        "," + this.rightBottomPosition.y.ToString("#0.0000") +
                        "," + this.rightBottomPosition.z.ToString("#0.0000") + ")" + "\n";

            logResult += "#scan track:" + "\n";
            for (int i = 0; i < this.trackDatas.Count; i++)
            {
                logResult += "-Time:" + this.trackDatas[i].trackTime.ToString("#0.000");
                if (trackDatas[i].isLegal)
                {
                    logResult += ",-Position:(" + this.trackDatas[i].trackPosition.x.ToString("#0.0000") +
                        "," + this.trackDatas[i].trackPosition.y.ToString("#0.0000") +
                        "," + this.trackDatas[i].trackPosition.z.ToString("#0.0000") + ")" + "\n";
                }
                else
                {
                    logResult += ",-Position:NaN" + "\n";
                }
            }

            logResult += "#endlog";
            logResult += "\n";
            return logResult;
        }
    }
}
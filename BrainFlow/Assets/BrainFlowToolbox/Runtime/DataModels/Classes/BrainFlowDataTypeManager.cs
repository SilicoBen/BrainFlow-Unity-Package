using System;
using System.Collections.Generic;
using BrainFlowToolbox.Runtime.DataVisualization;
using BrainFlowToolbox.Runtime.Enumerators;

namespace BrainFlowToolbox.Runtime.DataModels.Classes
{
    [Serializable]
    public class BrainFlowDataTypeManager
    {
        public BrainFlowDataType dataType;
        public int[] channelIds;
        public BrainFlowChannelTypeDashboard channelDashboard;
        public List<BrainFlowSingleChannelDataStream> channelDataStreamers;
        public List<BrainFlowSingleChannelDataStreamVisualizer> channelDataStreamVisualizers;
    }
}
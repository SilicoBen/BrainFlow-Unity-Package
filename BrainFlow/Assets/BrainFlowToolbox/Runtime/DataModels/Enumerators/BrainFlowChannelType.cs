using System;

namespace BrainFlowToolbox.Runtime.DataModels.Enumerators
{
    [Serializable]
    public enum BrainFlowChannelType
    {
        EEG,
        EXG,
        EMG,
        ECG,
        EOG,
        EDA,
        PPG,
        Accel,
        Analog,
        Gyro,
        Temperature,
        Resistance,
        Other
    }
}
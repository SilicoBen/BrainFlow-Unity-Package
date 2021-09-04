using System;

namespace BrainFlowToolbox.Runtime.Enumerators
{
    [Serializable]
    public enum BrainFlowDataType
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
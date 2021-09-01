using brainflow;
using UnityEngine;

namespace BrainFlowToolbox
{
    [CreateAssetMenu(fileName = "BrainFlowSessionProfile", menuName = "BrainFlow/BrainFlowSessionProfile", order = 0)]
    public class BrainFlowSessionProfile : ScriptableObject
    {
        public BoardIds board;
        public string boardDataFileName;
    }
}
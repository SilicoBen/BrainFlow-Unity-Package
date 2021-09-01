using System.Collections;
using System.Collections.Generic;
using brainflow;
using BrainFlowToolbox;
using UnityEngine;

public static class BrainFlowUtilities
{
    private static BoardShim boardShim;
    private static int samplingRate = 0;
    private static BrainFlowInputParams brainFlowInputParams;
    public static int boardID;

    public static void StartSession(BoardIds board, string fileName = "brainflow_data")
    {
        try
        {
            BoardShim.set_log_file(fileName + "_log.txt");
            BoardShim.enable_dev_board_logger();

            brainFlowInputParams = new BrainFlowInputParams();
            boardID = (int) board;
            boardShim = new BoardShim(boardID, brainFlowInputParams);
            boardShim.prepare_session();
            boardShim.start_stream(450000, "file://" + fileName + " .csv:w");
            samplingRate = BoardShim.get_sampling_rate(boardID);
            Debug.Log("BrainFlow: Session Started Successfully");
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
            Debug.Log("BrainFlow: Unable to Start Session");
        }
    }

    public static void StartSessionFromSessionProfile(BrainFlowSessionProfile sessionProfile)
    {
        try
        {
            BoardShim.set_log_file(sessionProfile.boardDataFileName + "_log.txt");
            BoardShim.enable_dev_board_logger();

            brainFlowInputParams = new BrainFlowInputParams();
            boardID = (int) sessionProfile.board;
            boardShim = new BoardShim(boardID, brainFlowInputParams);
            boardShim.prepare_session();
            boardShim.start_stream(450000, "file://" + sessionProfile.boardDataFileName + " .csv:w");
            samplingRate = BoardShim.get_sampling_rate(boardID);
            Debug.Log("BrainFlow: Session Started Successfully");
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
            Debug.Log("BrainFlow: Unable to Start Session");
        }
    }

    public static GameObject CreateDataStreamGameObject()
    }
        return null;
    }

    // EndSession calls release_session and ensures that all resources correctly released
    public static void EndSession()
    {
        if (boardShim == null)
        {
            Debug.Log("BrainFlow: Tried to end Session, but no Session was found!");
            return;
        }
        
        try
        {
            boardShim.release_session();
        }
        catch (BrainFlowException e)
        {
            Debug.Log(e);
        }
        Debug.Log("BrainFlow: Session has Ended");
    }
}

//using GooglePlayGames;
//using GooglePlayGames.BasicApi;
//using GooglePlayGames.BasicApi.SavedGame;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Events;

namespace TigerForge
{
    public static class EasyFileSaveGPGSExtension
    {
        //public static bool IsWorking = false;

        //public static void SaveGPGS(this EasyFileSave easyFileSave)
        //{
        //    easyFileSave.OpenSavedGame(easyFileSave.SaveGPGS);
        //}

        //public static void LoadGPGS(this EasyFileSave easyFileSave)
        //{
        //    easyFileSave.OpenSavedGame(easyFileSave.LoadGPGS);
        //}

        //public static void AppendGPGS(this EasyFileSave easyFileSave, bool overwrite = true)
        //{
        //    easyFileSave.OpenSavedGame((openedGame) => easyFileSave.AppendGPGS(openedGame, overwrite));            
        //}

        //public static void DeleteGPGS(this EasyFileSave easyFileSave)
        //{
        //    easyFileSave.OpenSavedGame(easyFileSave.DeleteGPGS);
        //}

        //private static void SaveGPGS(this EasyFileSave easyFileSave, ISavedGameMetadata openedGame)
        //{
        //    try
        //    {
        //        BinaryFormatter bf = new BinaryFormatter();
        //        MemoryStream ms = new MemoryStream();
        //        bf.Serialize(ms, easyFileSave.storage);

        //        var update = new SavedGameMetadataUpdate.Builder()
        //                       .WithUpdatedDescription("Saved at " + DateTime.Now.ToString())
        //                       .WithUpdatedPlayedTime(openedGame.TotalTimePlayed.Add(TimeSpan.FromHours(1)))
        //                       .Build();

        //        PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openedGame, update, ms.ToArray(), (status, updated) =>
        //        {
        //            ms.Close();
        //            easyFileSave.Dispose();
        //            openedGame = null;

        //            Debug.Log($"[Easy File Save GPGS] Save : {status}");
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        easyFileSave.Error = "[Easy File Save GPGS] This system exeption has been thrown during saving: " + e.Message;
        //    }
        //}

        //private static void LoadGPGS(this EasyFileSave easyFileSave, ISavedGameMetadata openedGame)
        //{
        //    try
        //    {
        //        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(openedGame, (status, data) =>
        //        {
        //            if (data != null && data.Length > 0)
        //            {
        //                BinaryFormatter bf = new BinaryFormatter();
        //                MemoryStream ms = new MemoryStream(data);
        //                easyFileSave.storage = (Dictionary<string, object>)bf.Deserialize(ms);
        //                ms.Close();
        //            }
        //            else
        //            {
        //               easyFileSave.storage = new Dictionary<string, object>();
        //            }

        //            object[] values = easyFileSave.storage.Values.ToArray();
        //            for (int i = 0; i < values.Length; i++)
        //                Debug.Log(values[i].ToString());

        //            Debug.Log($"[Easy File Save GPGS] Load1 : {status}");
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        easyFileSave.Error = "[Easy File Save GPGS] This system exeption has been thrown during loading: " + e.Message;
        //    }
        //}

        //private static void AppendGPGS(this EasyFileSave easyFileSave, ISavedGameMetadata openedGame, bool overwrite)
        //{
        //    try
        //    {
        //        Dictionary<string, object> fileStorage;

        //        PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(openedGame, (status, data) =>
        //        {
        //            BinaryFormatter bf;
        //            MemoryStream ms;

        //            if (data != null && data.Length > 0)
        //            {
        //                bf = new BinaryFormatter();
        //                ms = new MemoryStream(data);
        //                fileStorage = (Dictionary<string, object>)bf.Deserialize(ms);
        //                ms.Close();

        //                foreach (KeyValuePair<string, object> item in easyFileSave.storage)
        //                {
        //                    if (fileStorage.ContainsKey(item.Key) && overwrite)
        //                        fileStorage[item.Key] = item.Value;
        //                    else
        //                        fileStorage.Add(item.Key, item.Value);
        //                }
        //            }
        //            else
        //            {
        //                fileStorage = easyFileSave.storage;
        //            }

        //            bf = new BinaryFormatter();
        //            ms = new MemoryStream();
        //            bf.Serialize(ms, fileStorage);

        //            var update = new SavedGameMetadataUpdate.Builder()
        //                           .WithUpdatedDescription("Saved at " + DateTime.Now.ToString())
        //                           .WithUpdatedPlayedTime(openedGame.TotalTimePlayed.Add(TimeSpan.FromHours(1)))
        //                           .Build();

        //            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(openedGame, update, ms.ToArray(), (status, updated) =>
        //            {
        //                ms.Close();
        //                easyFileSave.Dispose();

        //                Debug.Log($"[Easy File Save GPGS] Append : {status}");
        //            });
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //       easyFileSave.Error = "[Easy File Save GPGS] This system exeption has been thrown during loading: " + e.Message;
        //    }
        //}

        //private static void DeleteGPGS(this EasyFileSave easyFileSave, ISavedGameMetadata openedGame)
        //{
        //    PlayGamesPlatform.Instance.SavedGame.Delete(openedGame);
        //    Debug.Log($"[Easy File Save GPGS] Delete : Success");
        //}

        //private static void OpenSavedGame(this EasyFileSave easyFileSave, UnityAction<ISavedGameMetadata> onOpened)
        //{
        //    PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(easyFileSave.fileName,
        //        DataSource.ReadNetworkOnly,
        //        ConflictResolutionStrategy.UseLastKnownGood,
        //        (status, opened) =>
        //        {
        //            if (opened != null)
        //            {
        //                GooglePlayGames.OurUtils.Logger.d("Opened file: " + opened);
        //                Debug.Log($"[Easy File Save GPGS] Open : {status}, Name : {opened}");

        //                onOpened?.Invoke(opened);
        //            }
        //            else
        //            {
        //                easyFileSave.Error = "[Easy File Save GPGS] Cannot open saved game";
        //            }
        //        });
        //}
    }
}

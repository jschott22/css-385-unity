/*
GameInfo.cs
Contributor(s): Jake Schott
*/

using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

public class GameInfo : MonoBehaviour
{
    private static readonly string SAVE_FOLDER = Application.dataPath + "/saves/";

    private int scenario_number = 1;
    private Vector3 player_position = Vector3.zero;
    private Vector3 player_rotation = Vector3.zero;

    public GameObject player;
    public TMP_Text scenario_txt;
    public TMP_Text plr_pos_txt;
    public TMP_Text progress_saved;

    private void Awake()
    {
        if (Directory.Exists(SAVE_FOLDER) == false)
        {
            Directory.CreateDirectory(SAVE_FOLDER);
        }

        if (File.Exists(SAVE_FOLDER + "/game_info.txt") == false)
        {
            save();
        }
        else
        {
            load();
        }
    }

    private void load()
    {
        SaveInfo to_load = JsonUtility.FromJson<SaveInfo>(File.ReadAllText(SAVE_FOLDER + "/game_info.txt"));
        player_position = to_load.current_position;
        player_rotation = to_load.current_rotation;
        scenario_number = to_load.current_scenario;

        player.transform.GetComponent<CameraMove>().setPrevPos(player_rotation);

        player.transform.position = player_position;
        player.transform.rotation = Quaternion.Euler(0.0f, player_rotation.y, 0.0f);
        player.transform.GetChild(0).rotation = Quaternion.Euler(player_rotation.x, 0.0f, 0.0f);
    }

    private void save()
    {
        SaveInfo temp_save = new SaveInfo()
        {
            current_scenario = scenario_number,
            current_position = player_position,
            current_rotation = player_rotation
        };
        string json_temp_save = JsonUtility.ToJson(temp_save);
        File.WriteAllText(SAVE_FOLDER + "/game_info.txt", json_temp_save);
    }

    private void Update()
    {
        Vector3 curr_pos = player.transform.position;
        curr_pos.x = Mathf.Round(curr_pos.x * 10.0f) / 10.0f;
        curr_pos.y = Mathf.Round(curr_pos.y * 10.0f) / 10.0f;
        curr_pos.z = Mathf.Round(curr_pos.z * 10.0f) / 10.0f;
        plr_pos_txt.SetText("POSITION: (" + curr_pos.x + ", " + curr_pos.y + ", " + curr_pos.z + ")");

        Vector2 curr_rot = new Vector2(player.transform.rotation.eulerAngles.y, player.transform.GetChild(0).transform.localRotation.eulerAngles.x);

        player_position = curr_pos;
        player_rotation = curr_rot;
    
        if (Input.GetKeyDown(KeyCode.N))
        {
            scenario_number++;
        }

        scenario_txt.SetText("SCENARIO: " + scenario_number);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            save();
            StopAllCoroutines();
            StartCoroutine(displayProgressSaved());
        }
    }

    IEnumerator displayProgressSaved()
    {
        progress_saved.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        progress_saved.gameObject.SetActive(false);
    }

    private class SaveInfo
    {
        public int current_scenario;
        public Vector3 current_position;
        public Vector2 current_rotation;
    }
}

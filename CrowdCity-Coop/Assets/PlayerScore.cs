using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Player;
using TMPro;
using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public TextMeshProUGUI placeText, amountText, nicknameText;
    public Image background;
    
    private Leader _leader;
    private bool _isLocal;

    public void Init(Leader l)
    {
        _leader = l;
        placeText.text = _leader.LeaderboardPlace.ToString();
        amountText.text = "x" + _leader.Followers.ToString();
        nicknameText.text = _leader.PlayerConfigurations.nickname;
    }

    public void UpdateInfo()
    {
        placeText.text = _leader.LeaderboardPlace.ToString();
        amountText.text = "x" + _leader.Followers.ToString();
    }
}

using System.Linq;
using TheLostSpirit.Application.UseCase.Formula;
using TheLostSpirit.Application.UseCase.Portal;
using TheLostSpirit.Context;
using TheLostSpirit.Context.EntryPoint.Playground;
using TheLostSpirit.Context.Formula;
using TheLostSpirit.Context.Player;
using TheLostSpirit.Context.Portal;
using TheLostSpirit.Domain;
using TheLostSpirit.Identity.SpecificationID;
using TheLostSpirit.Infrastructure;
using UnityEngine;


public class MapTestContext : MonoBehaviour
{
    [SerializeField]
    PlayerContext _playerContext;

    [SerializeField]
    PlayerInstanceContext _playerInstanceContext;

    [SerializeField]
    UserInputContext _userInputContext;
    // Start is called before the first frame update
    void Awake()
    {
        Construct();

    }
    MapTestContext Construct()
    {
        AppScope.Initialize(new EventBus());

        _userInputContext.Construct();
        _playerContext.Construct();
        _playerInstanceContext.Construct(_playerContext,_userInputContext);

        return this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

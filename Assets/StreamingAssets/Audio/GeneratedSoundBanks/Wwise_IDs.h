/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID AMB_DESERT_PLAY = 2160078966U;
        static const AkUniqueID AMB_EXT_LVL01_DESERT_GLOBAL = 26480583U;
        static const AkUniqueID AMB_EXT_LVL01_DESERT_KIOSK = 1449773595U;
        static const AkUniqueID AMB_EXT_LVL01_DESERT_VIEW = 3505954145U;
        static const AkUniqueID AMB_EXT_LVL02_ENTRANCE = 1324574823U;
        static const AkUniqueID AMB_EXT_LVL02_PLACE = 2530427570U;
        static const AkUniqueID AMB_INT_LVL01 = 3602364923U;
        static const AkUniqueID AMB_INT_TEMPLE_END = 4294699005U;
        static const AkUniqueID AMB_INT_TUNNEL_END = 3181597140U;
        static const AkUniqueID ASTRO_MOVING_PLAY = 3822072736U;
        static const AkUniqueID ASTRO_MOVING_STOP = 2165331746U;
        static const AkUniqueID CHA_BREATH_EXPI = 186543767U;
        static const AkUniqueID CHA_BREATH_INSPI = 1039592238U;
        static const AkUniqueID CHA_BREATH_PLAY = 182011575U;
        static const AkUniqueID CHA_DEATH_PLAY = 872937223U;
        static const AkUniqueID CHA_FOOTSTEPS_PLAY = 1343142182U;
        static const AkUniqueID CHA_HURT = 1546453947U;
        static const AkUniqueID CHA_IDLE = 3135834134U;
        static const AkUniqueID CHA_RESPAWN = 1215500860U;
        static const AkUniqueID CHA_RUN = 3845266727U;
        static const AkUniqueID CHA_WALK = 300025675U;
        static const AkUniqueID CHECKPOINT_FOUND = 3822733366U;
        static const AkUniqueID LVL01_FLYING_SPHERE_BIG_PLAY = 1047307714U;
        static const AkUniqueID LVL01_FLYING_SPHERE_SMALL_PLAY = 3897898391U;
        static const AkUniqueID LVL02_PLATEFORM_PLACE = 213028944U;
        static const AkUniqueID MUSIC_LVL01_BRIDGE1 = 771829065U;
        static const AkUniqueID MUSIC_LVL01_KIOSK1 = 2884165201U;
        static const AkUniqueID MUSIC_LVL01_KIOSK2 = 2884165202U;
        static const AkUniqueID MUSIC_LVL01_KIOSK3 = 2884165203U;
        static const AkUniqueID MUSIC_LVL01_TEMPLE = 503471100U;
        static const AkUniqueID MUSIC_LVL02_CITY1 = 3897915588U;
        static const AkUniqueID MUSIC_LVL02_SWITCH_EVENT = 3454836319U;
        static const AkUniqueID MUSIC_PLAY = 202194903U;
        static const AkUniqueID MUSIC_PLAY_GLOBAL = 4260531187U;
        static const AkUniqueID MUSIC_STOP_FADE = 1426984690U;
        static const AkUniqueID SUN_FEEDBACK_PLAY = 2337456342U;
        static const AkUniqueID TEMPLEDOOR_OPEN = 289201289U;
        static const AkUniqueID UI_CLICKED = 3391821525U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace AMBS
        {
            static const AkUniqueID GROUP = 3537656742U;

            namespace STATE
            {
                static const AkUniqueID ENTRANCE_LVL02 = 2445910262U;
                static const AkUniqueID EXT_LVL01_DESERT_GLOBAL = 2890487870U;
                static const AkUniqueID EXT_LVL01_DESERT_KIOSK = 3857277276U;
                static const AkUniqueID EXT_LVL01_DESERT_VIEW = 3003725028U;
                static const AkUniqueID INT_LVL01 = 3139526458U;
                static const AkUniqueID INT_TEMPLE_END = 3293789522U;
                static const AkUniqueID INT_TUNNEL_END = 2033314023U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PLACE_LVL02 = 1042178311U;
            } // namespace STATE
        } // namespace AMBS

        namespace GENERALSTATES
        {
            static const AkUniqueID GROUP = 874109109U;

            namespace STATE
            {
                static const AkUniqueID ISHURTING = 1898407218U;
                static const AkUniqueID ISIDLE = 2139983939U;
                static const AkUniqueID ISWALKING = 3629409974U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace GENERALSTATES

        namespace MENU_STATE
        {
            static const AkUniqueID GROUP = 3941853002U;

            namespace STATE
            {
                static const AkUniqueID INGAME = 984691642U;
                static const AkUniqueID INMENU = 3374585465U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace MENU_STATE

        namespace MUSIC_STATE
        {
            static const AkUniqueID GROUP = 3826569560U;

            namespace STATE
            {
                static const AkUniqueID MENU_STATE = 3941853002U;
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PART01 = 2201513847U;
                static const AkUniqueID PART02 = 2201513844U;
                static const AkUniqueID PART03 = 2201513845U;
                static const AkUniqueID PART04 = 2201513842U;
                static const AkUniqueID PART05 = 2201513843U;
                static const AkUniqueID PART06 = 2201513840U;
                static const AkUniqueID PART07 = 2201513841U;
            } // namespace STATE
        } // namespace MUSIC_STATE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace BREATH_SWITCH
        {
            static const AkUniqueID GROUP = 1231199118U;

            namespace SWITCH
            {
                static const AkUniqueID ISHURTING = 1898407218U;
                static const AkUniqueID ISIDLE = 2139983939U;
                static const AkUniqueID ISRUNNING = 4119476486U;
                static const AkUniqueID ISWALKING = 3629409974U;
            } // namespace SWITCH
        } // namespace BREATH_SWITCH

        namespace FOOTSTEP_FLOOR
        {
            static const AkUniqueID GROUP = 997064848U;

            namespace SWITCH
            {
                static const AkUniqueID SAND = 803837735U;
                static const AkUniqueID STONE = 1216965916U;
            } // namespace SWITCH
        } // namespace FOOTSTEP_FLOOR

        namespace GENERAL_MUSIC_SWITCH
        {
            static const AkUniqueID GROUP = 2502520850U;

            namespace SWITCH
            {
                static const AkUniqueID LVL01 = 1709218600U;
                static const AkUniqueID LVL02 = 1709218603U;
            } // namespace SWITCH
        } // namespace GENERAL_MUSIC_SWITCH

        namespace GLOBAL_SWITCH
        {
            static const AkUniqueID GROUP = 3921090687U;

            namespace SWITCH
            {
                static const AkUniqueID ISHURTING2 = 1392790916U;
                static const AkUniqueID NORMAL = 1160234136U;
            } // namespace SWITCH
        } // namespace GLOBAL_SWITCH

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID AMB_RTPC = 2682991203U;
        static const AkUniqueID BREATH_RTPC = 748151351U;
        static const AkUniqueID RTPC_ASTROSIDECHAIN = 2972377204U;
        static const AkUniqueID RTPC_DISTANCE_SUN = 2257099397U;
        static const AkUniqueID RTPC_MAIN_VOLUME = 1856500933U;
        static const AkUniqueID RTPC_MUSIC_VOLUME = 1596647065U;
        static const AkUniqueID RTPC_PLATEFORM_VELOCITY = 3829022635U;
        static const AkUniqueID RTPC_SFX_VOLUME = 932301089U;
        static const AkUniqueID RTPC_SUN_VELOCITY = 582120507U;
        static const AkUniqueID RTPC_WINDGAIN = 1687410496U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID ALL = 1100754030U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AMBIANCE_BUS = 3161292138U;
        static const AkUniqueID BREATHING_BUS = 3834658866U;
        static const AkUniqueID FOLEYS_BUS = 1306998534U;
        static const AkUniqueID FX = 1802970371U;
        static const AkUniqueID INT_LVL01_ACOUSTIC = 1571012312U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSIC_BUS = 2680856269U;
    } // namespace BUSSES

    namespace AUX_BUSSES
    {
        static const AkUniqueID INT_LVL01_FX = 2514557383U;
        static const AkUniqueID INT_LVL02_ENTRANCE = 2582645766U;
        static const AkUniqueID SIDECHAIN_MUSIC = 1657183839U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__

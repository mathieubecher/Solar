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
        static const AkUniqueID AMB_INT_LVL01 = 3602364923U;
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
        static const AkUniqueID LVL01_FLYING_SPHERE_BIG_PLAY = 1047307714U;
        static const AkUniqueID LVL01_FLYING_SPHERE_SMALL_PLAY = 3897898391U;
        static const AkUniqueID MUSIC_LVL01_BRIDGE1 = 771829065U;
        static const AkUniqueID MUSIC_LVL01_KIOSK1 = 2884165201U;
        static const AkUniqueID MUSIC_LVL01_KIOSK2 = 2884165202U;
        static const AkUniqueID MUSIC_LVL01_KIOSK3 = 2884165203U;
        static const AkUniqueID MUSIC_LVL01_TEMPLE = 503471100U;
        static const AkUniqueID MUSIC_PLAY = 202194903U;
        static const AkUniqueID SUN_FEEDBACK_PLAY = 2337456342U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace AMB_LVL01
        {
            static const AkUniqueID GROUP = 416592333U;

            namespace STATE
            {
                static const AkUniqueID EXT_LVL01_DESERT_GLOBAL = 2890487870U;
                static const AkUniqueID EXT_LVL01_DESERT_KIOSK = 3857277276U;
                static const AkUniqueID EXT_LVL01_DESERT_VIEW = 3003725028U;
                static const AkUniqueID INT_LVL01 = 3139526458U;
                static const AkUniqueID NONE = 748895195U;
            } // namespace STATE
        } // namespace AMB_LVL01

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

        namespace MUSIC_STATE
        {
            static const AkUniqueID GROUP = 3826569560U;

            namespace STATE
            {
                static const AkUniqueID NONE = 748895195U;
                static const AkUniqueID PART01 = 2201513847U;
                static const AkUniqueID PART02 = 2201513844U;
                static const AkUniqueID PART03 = 2201513845U;
                static const AkUniqueID PART04 = 2201513842U;
                static const AkUniqueID PART05 = 2201513843U;
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
        static const AkUniqueID RTPC_PLATEFORM_VELOCITY = 3829022635U;
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
        static const AkUniqueID SIDECHAIN_MUSIC = 1657183839U;
    } // namespace AUX_BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__

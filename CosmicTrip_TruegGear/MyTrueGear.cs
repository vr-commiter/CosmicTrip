using System.Threading;
using TrueGearSDK;
using System.Linq;
using CosmicTrip_TruegGear;
using I2.Loc.SimpleJSON;


namespace MyTrueGear
{
    public class TrueGearMod
    {
        public static float angle = 0;
        public static float ver = 0;
        private static TrueGearPlayer _player = null;

        private static ManualResetEvent lefthandfrisbeeMRE = new ManualResetEvent(false);
        private static ManualResetEvent righthandfrisbeeMRE = new ManualResetEvent(false);
        private static ManualResetEvent lefthandinhaleMRE = new ManualResetEvent(false);
        private static ManualResetEvent righthandinhaleMRE = new ManualResetEvent(false);


        public void LeftHandFrisbee()
        {
            while(true)
            {
                lefthandfrisbeeMRE.WaitOne();
                _player.SendPlay("LeftHandFrisbee");
                Thread.Sleep(70);
            }            
        }

        public void RightHandFrisbee()
        {
            while (true)
            {
                righthandfrisbeeMRE.WaitOne();
                _player.SendPlay("RightHandFrisbee");
                Thread.Sleep(70);
            }
        }

        public void LeftHandInhale()
        {
            while (true)
            {
                lefthandinhaleMRE.WaitOne();
                _player.SendPlay("LeftHandInhale");
                Thread.Sleep(70);
            }
        }

        public void RightHandInhale()
        {
            while (true)
            {
                righthandinhaleMRE.WaitOne();
                _player.SendPlay("RightHandInhale");
                Thread.Sleep(70);
            }
        }

        public TrueGearMod() 
        {
            _player = new TrueGearPlayer("427240","Cosmic Trip");
            _player.PreSeekEffect("DefaultDamage");
            _player.Start();
            new Thread(new ThreadStart(this.LeftHandFrisbee)).Start();
            new Thread(new ThreadStart(this.RightHandFrisbee)).Start();
            new Thread(new ThreadStart(this.LeftHandInhale)).Start();
            new Thread(new ThreadStart(this.RightHandInhale)).Start();
        }    


        public void Play(string Event)
        { 
            _player.SendPlay(Event);
        }

        
        public void PlayAngle(string tmpEvent, float tmpAngle, float tmpVertical)
        {
            try
            {
                float angle = (tmpAngle - 22.5f) > 0f ? tmpAngle - 22.5f : 360f - tmpAngle;
                int horCount = (int)(angle / 45) + 1;

                int verCount = tmpVertical > 0.1f ? -4 : tmpVertical < -0.5f ? 8 : 0;


                EffectObject oriObject = _player.FindEffectByUuid(tmpEvent);

                TrueGearSDK.SimpleJSON.JSONObject jsonObject = (TrueGearSDK.SimpleJSON.JSONObject)oriObject.ToJsonObject().Clone();

                EffectObject rootObject = EffectObject.ToObject(jsonObject);


                foreach (TrackObject track in rootObject.trackList)
                {
                    if (track.action_type == ActionType.Shake)
                    {
                        for (int i = 0; i < track.index.Length; i++)
                        {
                            if (verCount != 0)
                            {
                                track.index[i] += verCount;
                            }
                            if (horCount < 8)
                            {
                                if (track.index[i] < 50)
                                {
                                    int remainder = track.index[i] % 4;
                                    if (horCount <= remainder)
                                    {
                                        track.index[i] = track.index[i] - horCount;
                                    }
                                    else if (horCount <= (remainder + 4))
                                    {
                                        var num1 = horCount - remainder;
                                        track.index[i] = track.index[i] - remainder + 99 + num1;
                                    }
                                    else
                                    {
                                        track.index[i] = track.index[i] + 2;
                                    }
                                }
                                else
                                {
                                    int remainder = 3 - (track.index[i] % 4);
                                    if (horCount <= remainder)
                                    {
                                        track.index[i] = track.index[i] + horCount;
                                    }
                                    else if (horCount <= (remainder + 4))
                                    {
                                        var num1 = horCount - remainder;
                                        track.index[i] = track.index[i] + remainder - 99 - num1;
                                    }
                                    else
                                    {
                                        track.index[i] = track.index[i] - 2;
                                    }
                                }
                            }
                        }
                        if (track.index != null)
                        {
                            track.index = track.index.Where(i => !(i < 0 || (i > 19 && i < 100) || i > 119)).ToArray();
                        }
                    }
                    else if (track.action_type == ActionType.Electrical)
                    {
                        for (int i = 0; i < track.index.Length; i++)
                        {
                            if (horCount <= 4)
                            {
                                track.index[i] = 0;
                            }
                            else
                            {
                                track.index[i] = 100;
                            }
                            if (horCount == 1 || horCount == 8 || horCount == 4 || horCount == 5)
                            {
                                track.index = new int[2] { 0, 100 };
                            }

                        }
                    }
                }
                _player.SendPlayEffectByContent(rootObject);
            }
            catch (System.Exception ex)
            {
                _player.SendPlay(tmpEvent);
                Plugin.Log.LogError(ex);
            }
        }



        public void StartLeftHandFrisbee()
        {
            lefthandfrisbeeMRE.Set();
        }

        public void StopLeftHandFrisbee()
        {
            lefthandfrisbeeMRE.Reset();
        }

        public void StartRightHandFrisbee()
        {
            righthandfrisbeeMRE.Set();
        }

        public void StopRightHandFrisbee()
        {
            righthandfrisbeeMRE.Reset();
        }

        public void StartLeftHandInhale()
        {
            lefthandinhaleMRE.Set();
        }

        public void StopLeftHandInhale()
        {
            lefthandinhaleMRE.Reset();
        }

        public void StartRightHandInhale()
        {
            righthandinhaleMRE.Set();
        }

        public void StopRightHandInhale()
        {
            righthandinhaleMRE.Reset();
        }

    }
}

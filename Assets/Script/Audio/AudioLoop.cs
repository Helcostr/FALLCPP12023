using UnityEngine;
using UnityEngine.Windows;

public class AudioLooper {
    private int position;
    private int introLength;
    private int loopLength;
    private AudioClip stream;
    [SerializeField]
    private float[] intro;
    [SerializeField]
    private float[] loop;
    public AudioLooper(AudioClip introClip, AudioClip loopClip) {
        position = 0;
        intro = getSamples(introClip);
        loop = getSamples(loopClip);
        introLength = introClip.samples * introClip.channels;
        loopLength = loopClip.samples * loopClip.channels;
        stream = AudioClip.Create("Stream", introClip.samples * 2, introClip.channels, introClip.frequency, true, OnLoopAudioRead);
    }
    private float[] getSamples(AudioClip input) {
        float[] output = new float[input.samples * input.channels];
        input.GetData(output, 0);
        return output;
    }
    void OnLoopAudioRead(float[] data) {
        int count = 0;
        while (count < data.Length) {
            if (intro != null)
                if (position < introLength)
                    data[count] = intro[position];
                else if  (position == introLength) {
                    intro = null;
                    position = 0;
                }
            if (intro == null) {
                data[count] = loop[position % loopLength];
            }
            position++;
            count++;
        }
        if (intro == null && position > loopLength)
            position -= loopLength;
    }
    public void initPlayer(AudioSource audioSource) {
        audioSource.clip = stream;
        audioSource.loop = true;
        audioSource.Play();
    }
}

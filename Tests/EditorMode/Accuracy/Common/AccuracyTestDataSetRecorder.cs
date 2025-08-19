using System.Linq;
using UnistrokeGestureRecognition;
using UnistrokeGestureRecognition.Example;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class AccuracyTestDataSetRecorder : MonoBehaviour {
        [field: SerializeField] public AccuracyTestDataSet DataSet { get; private set; }

    [SerializeField, Range(0.6f, 1f)] private float _minimumScore = 0.8f;

    [SerializeField] private PathDrawerBase _pathDrawer;
    [SerializeField] private NameController _nameController;

    private Camera _camera;
    private IGestureRecorder _gestureRecorder;

    private IGestureRecognizer<AccuracyTestGesturePattern> _recognizer;

    private JobHandle? _recognizeJob;

    private void Awake() {
        _gestureRecorder = new GestureRecorder(1024, 0);
        _recognizer = new GestureRecognizer<AccuracyTestGesturePattern>(DataSet.Patterns, 256);
    }

    private void Start() {
        _pathDrawer.Show();
        _camera = Camera.main;
    }

    private void OnDestroy() {
        _recognizer.Dispose();
        _gestureRecorder.Dispose();
    }

    private void Update() {
        if (Input.GetKeyUp(KeyCode.Mouse0)) {
            if (_gestureRecorder.Length > 10) {
                RecognizeRecordedGesture();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) {
            Clear();
        }

        RecordNewPoint();
    }

    private void LateUpdate() {
        if (!_recognizeJob.HasValue)
            return;

        _recognizeJob.Value.Complete();

        RecognizeResult<AccuracyTestGesturePattern> result = _recognizer.Result;

        if (result.Score >= _minimumScore) {
            AccuracyTestGesturePattern recognizedPattern = result.Pattern;
            _nameController.Set($"{result.Score:0.00}");
            DataSet.Records.Add(new AccuracyGestureRecord(result.Pattern, _gestureRecorder.Path.Select<float2, Vector2>(p => p).ToList()));
        }

        _recognizeJob = null;
    }

    private void RecognizeRecordedGesture() {
        _recognizeJob = _recognizer.ScheduleRecognition(_gestureRecorder.Path);
    }

    private void Clear() {
        _nameController.Clear();
        _pathDrawer.Clear();
        _gestureRecorder.Reset();
    }

    private void RecordNewPoint() {
        var screenPosition = Input.mousePosition;
        Vector2 point = _camera.ScreenToWorldPoint(screenPosition);

        if (Input.GetKey(KeyCode.Mouse0)) {
            _gestureRecorder.RecordPoint(new Vector2(screenPosition.x, screenPosition.y));
            _pathDrawer.AddPoint(point);
        }
    }

    private void OnValidate() {
        if (Application.isPlaying && _recognizer != null) {
            _recognizer.Dispose();
            _recognizer = new GestureRecognizer<AccuracyTestGesturePattern>(DataSet.Patterns);
        }
    }
}
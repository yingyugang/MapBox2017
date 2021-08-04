/*
using Mapbox.Unity.Map;
using Mapbox.Utils;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
namespace PE.PlatMapbox
{
	/// <summary>
	/// マップの操作
	/// </summary>
	public class MapPresenter : MonoBehaviour
	{
		[SerializeField]
		Transform target;
		[SerializeField]
		Vector3 offset = new Vector3(0, 100, 0);
		[SerializeField]
		AbstractMap _map;
		[SerializeField]
		Vector2d geoCoord;
		[SerializeField]
		Camera earthCamera;
		[SerializeField]
		ObservableEventTrigger touchHandler = null;
		[SerializeField]
		int dragFactor = 10000;
		[SerializeField]
		GameObject markerPrefab;
		List<GeocoorMarker> markers;
		[SerializeField]
		RectTransform mapRectTrans;
		[SerializeField]
		Canvas mapCanvas;

		Transform earthCameraTrans;
		float? lastPinchDistance;
		Vector2? preDirect;

		void Start()
		{
			earthCameraTrans = earthCamera.transform;
			markers = new List<GeocoorMarker>();
			geoCoord = new Vector2d(35.69, 139.69);
			_map.Initialize(geoCoord, _map.AbsoluteZoom);
			touchHandler.OnDragAsObservable()
			   .Where(_ => Input.touchCount < 2)
			   .Do(pointerEvent =>
			   {
				   var factor = 32 * Mathf.Pow(2, (_map.Zoom - 16)) * dragFactor;
				   var direct = earthCameraTrans.right * pointerEvent.delta.x + earthCameraTrans.up * pointerEvent.delta.y;
				   var deltaGeoCoord = new Vector2d(direct.z / factor, direct.x / factor);
				   if (geoCoord.x - deltaGeoCoord.x > -90 && geoCoord.x - deltaGeoCoord.x < 90)
				   {
					   geoCoord -= deltaGeoCoord;
				   }
			   })
			   .Subscribe();

			touchHandler.OnPointerClickAsObservable()
				.Do(pointerEvent =>
				{
					if (pointerEvent.dragging)
					{
						return;
					}
					Vector2 localPoint;
					if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mapRectTrans, pointerEvent.position, mapCanvas.worldCamera,out localPoint))
					{
						var targetPos = earthCamera.transform.position + new Vector3(localPoint.x / 160,0, localPoint.y / 160) * earthCamera.orthographicSize;
						RaycastHit raycastHit;
						if (Physics.Raycast(targetPos,earthCameraTrans.forward,out raycastHit,1 << 11))
						{
							var position = new Vector3(raycastHit.point.x, 0, raycastHit.point.z);
							var marker = Instantiate(markerPrefab, position, Quaternion.identity);
							marker.SetActive(true);
							var geocoord = _map.WorldToGeoPosition(position);
							markers.Add(new GeocoorMarker(geocoord,marker.transform));
						}
					}
				}).Subscribe();

			touchHandler.OnPointerDownAsObservable()
				.SelectMany(_ => touchHandler.UpdateAsObservable())
				.TakeUntil(touchHandler.OnPointerUpAsObservable())
				.Do(_ =>
				{
					if (Input.touchCount < 2)
					{
						lastPinchDistance = null;
						preDirect = null;
						return;
					}
					float moveDelta = 0;
					#region Googleマップのような回転
					var direct = (Input.GetTouch(1).position - Input.GetTouch(0).position).normalized;
					if (preDirect != null)
					{
						var delta = Vector2.SignedAngle(preDirect.GetValueOrDefault(), direct);
						if (Mathf.Abs(delta) > 2)
						{
							earthCameraTrans.transform.Rotate(Vector3.up, delta, Space.World);
							preDirect = direct;
							return;
						}
					}
					preDirect = direct;
					#endregion
					var pinchDistance = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude / Screen.dpi;
					if (lastPinchDistance.HasValue)
					{
						moveDelta = pinchDistance - lastPinchDistance.Value;
					}
					else
					{
						lastPinchDistance = pinchDistance;
					}
					var zoom = Mathf.Clamp(_map.Zoom + moveDelta / 3, 2, 22);
					_map.UpdateMap(zoom);
					UpdateMarkers();
				})
				.DoOnCompleted(() =>
				{
					lastPinchDistance = null;
					preDirect = null;
				})
				.RepeatUntilDestroy(touchHandler)
				.Subscribe();

#if UNITY_EDITOR || UNITY_STANDALONE
			// マウススクロールが動いてたら拡大縮小
			Observable.EveryUpdate()
				.TakeUntilDestroy(gameObject)
				.Subscribe(_ =>
				{
					var zoom = Mathf.Clamp(_map.Zoom + Input.mouseScrollDelta.y / 3, 2, 22);
					_map.UpdateMap(zoom);
					UpdateMarkers();
				});
#endif
		}

		void Update()
		{
			target.transform.localPosition = _map.GeoToWorldPosition(geoCoord);
#if UNITY_EDITOR || UNITY_STANDALONE
			if (Input.GetKey(KeyCode.A))
			{
				earthCameraTrans.transform.Rotate(Vector3.up, Time.deltaTime * 10, Space.World);
			}
			if (Input.GetKey(KeyCode.D))
			{
				earthCameraTrans.transform.Rotate(Vector3.up, -Time.deltaTime * 10, Space.World);
			}
#endif
		}

		void UpdateMarkers()
		{
			Vector3 distance;
			foreach (GeocoorMarker geocoorMarker in markers)
			{
				var position = _map.GeoToWorldPosition(geocoorMarker.geocoord);
				distance = position - earthCameraTrans.position;
				if (Mathf.Abs(distance.x) > earthCamera.orthographicSize || Mathf.Abs(distance.z) > earthCamera.orthographicSize)
				{
					continue;
				}
				geocoorMarker.marker.position = position;
			}
		}

		void LateUpdate()
		{
			earthCameraTrans.position = target.position + offset;
		}

		class GeocoorMarker
		{
			public Vector2d geocoord { get; set; }
			public Transform marker { get; set; }
			public GeocoorMarker(Vector2d geocoord, Transform marker)
			{
				this.geocoord = geocoord;
				this.marker = marker;
			}
		}
	}
}
*/
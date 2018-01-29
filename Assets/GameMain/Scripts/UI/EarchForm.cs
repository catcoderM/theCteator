using GameFramework;
//using GameFramework;
using UnityEngine;
using UnityGameFramework.Runtime;


namespace StarForce
{

	public class EarchForm : UGuiForm
	{
		[SerializeField]
		private GameObject eacrhImg = null;
		[SerializeField]
		private GameObject playerImg1 = null;
		[SerializeField]
		private GameObject playerImg2 = null;




		private ProcedureMenu m_ProcedureMenu = null;

		public void OnStartButtonClick()
		{
			m_ProcedureMenu.StartGame();
		}

		public void OnSettingButtonClick()
		{
			GameEntry.UI.OpenUIForm(UIFormId.SettingForm);
		}

		public void OnAboutButtonClick()
		{
			GameEntry.UI.OpenUIForm(UIFormId.AboutForm);
		}

		public void OnQuitButtonClick()
		{
			
		}

		#if UNITY_2017_3_OR_NEWER
		protected override void OnOpen(object userData)
		#else
		protected internal override void OnOpen(object userData)
		#endif
		{
			base.OnOpen(userData);

			Vector3 localPos = new Vector3(0, 0, 0);			
			this.playerImg1.gameObject.transform.localPosition = localPos;
			this.playerImg2.gameObject.transform.localPosition = localPos;
			Vector3 rotation = new Vector3(0, 0, 60);
			this.playerImg2.transform.Rotate(rotation);

			m_ProcedureMenu = (ProcedureMenu)userData;
			if (m_ProcedureMenu == null)
			{
				Log.Warning("ProcedureMenu is invalid when open MenuForm.");
				return;
			}

			//m_QuitButton.SetActive(Application.platform != RuntimePlatform.IPhonePlayer);
		}

		#if UNITY_2017_3_OR_NEWER
		protected override void OnClose(object userData)
		#else
		protected internal override void OnClose(object userData)
		#endif
		{
			m_ProcedureMenu = null;

			base.OnClose(userData);
		}




		string state = "0";
		Vector3 rotSpeed = new Vector3(0, 0, 90);
		float timer = 0;
		float earthStopTime = 1;
		int step = 60;
		int cur_step = 30;
		float minEarthRotationTime = 0.4f;


		#if UNITY_2017_3_OR_NEWER
		protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		#else
		protected internal override void OnUpdate(float elapseSeconds, float realElapseSeconds)
		#endif
		{
			base.OnUpdate(elapseSeconds, realElapseSeconds);

			//this.eacrhImg.gameObject.transform.Rotate(rotSpeed * Time.deltaTime);


			switch (state){
			case "0": // Earth Rotates
				Vector3 rot = rotSpeed * Time.deltaTime;
				rot.z = (int)rot.z;
				this.playerImg1.gameObject.transform.Rotate(rot);
				this.playerImg2.gameObject.transform.Rotate(rot);
				this.eacrhImg.gameObject.transform.Rotate(rot / 5);


				// Change State
				if (cur_step == 30){
					if (this.playerImg1.gameObject.transform.localEulerAngles.z < step){
						if (this.playerImg1.gameObject.transform.localEulerAngles.z > cur_step){
							cur_step += step;
							state = "1";
						}
					}
				}
				else if (this.playerImg1.gameObject.transform.localEulerAngles.z > cur_step){
					cur_step += step;
					state = "1";
				}

				break;
			

			case "1": // Earth Stops
				Debug.Log(cur_step);
				rot = rotSpeed * Time.deltaTime / 5;
				rot.z = (int)rot.z;
				this.eacrhImg.gameObject.transform.Rotate(rot);
				timer += Time.deltaTime;
				// Change State
				if (timer > earthStopTime){
					timer = 0;
					state = "0";
				}
				if (Input.GetKeyDown(KeyCode.Space)){
					state = "2";
					if (earthStopTime > minEarthRotationTime){
						earthStopTime -= 0.2f;
						timer = 0;
						Debug.Log("Took!!!!");
					}

					if (cur_step - step == 90 || cur_step == 90){
						state = "2";
						timer = 0;
					}
					else{
						state = "3";
						timer = 0;
					}

				}

				break;

			case "2":
				timer += Time.deltaTime;
				if (cur_step - step == 90){
					if (timer < 1.8){
						Vector3 pos = this.playerImg1.gameObject.transform.position;
						pos.y += 300 * Time.deltaTime;
						this.playerImg1.gameObject.transform.position = pos;
					}
					else{
						Quaternion rotat = new Quaternion(0, 0, 0, 0);
						this.playerImg1.gameObject.transform.localRotation = rotat;

						Vector3 localPos = new Vector3(0, 0, 0);

						this.playerImg1.gameObject.transform.localPosition = localPos;
						Vector3 rotation = new Vector3(0, 0, 90);
						this.playerImg1.gameObject.transform.Rotate(rotation);

						Debug.Log(this.gameObject.transform.eulerAngles);
						timer = 0;
						state = "0";
					}
				}
				if (cur_step == 90){
					if (timer < 1.8){
						Vector3 pos = this.playerImg2.gameObject.transform.position;
						pos.y += 300 * Time.deltaTime;
						this.playerImg2.gameObject.transform.position = pos;
					}
					else{

						Quaternion rotat = new Quaternion(0, 0, 0, 0);
						this.playerImg2.gameObject.transform.localRotation = rotat;

						Vector3 localPos = new Vector3(0, 0, 0);

						this.playerImg2.gameObject.transform.localPosition = localPos;

						Vector3 rotation = new Vector3(0, 0, 90);
						this.playerImg2.gameObject.transform.Rotate(rotation);



						Debug.Log(this.gameObject.transform.eulerAngles);
						timer = 0;
						state = "0";
					}
				}/*
				if (timer > 1.8){
					Vector3 rotat = new Vector3(0, 0, -101);
					this.playerImg1.gameObject.transform.Rotate(rotat);

					Vector3 localPos = new Vector3(0, 0, 0);

					this.playerImg1.gameObject.transform.localPosition = localPos;
					cur_step = 30;
					Debug.Log(this.gameObject.transform.eulerAngles);
					timer = 0;
					state = "0";

				}
				/*else{
					Debug.Log("Step 1");
					Vector3 pos = this.playerImg1.gameObject.transform.position;
					pos.y += 300 * Time.deltaTime;
					this.playerImg1.gameObject.transform.position = pos;
				}*/

				break;


			case "3":

				timer += Time.deltaTime;
				if (timer > 1.6){
					state = "0";
					timer = 0;
				}
				break;



			default:
				Debug.Log("Unknown State" + state);
				break;
			}
			if (cur_step >= 360){
				cur_step %= 360;
			}

		}
	}


}

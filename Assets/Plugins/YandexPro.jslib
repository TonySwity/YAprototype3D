mergeInto(LibraryManager.library, {
	Auth: function ()
	{
		initPlayer().then(_player => {
			if (_player.getMode() === 'lite') {
            // Игрок не авторизован.
				ysdk.auth.openAuthDialog().then(() => {
                    // Игрок успешно авторизован
					myGameInstance.SendMessage("YandexProvider", "LoadPlayerProgressData");
					initPlayer().catch(err => {
                        // Ошибка при инициализации объекта Player.
					});
				}).catch(() => {
                    // Игрок не авторизован.
					console.log("не авторизован");
				});
			}
		}).catch(err => {
        // Ошибка при инициализации объекта Player.
		});
	},

	CheckAuth: function() {
		if (player.getMode() === 'lite') {
			console.log("не авторизован");
		}
		else {
		console.log("авторизован");
		}
	},

	SaveYG: function (date)
	{
		//SaveCloud(UTF8ToString(date));
		console.log("SaveCloud");
		var dateString = UTF8ToString(date);
		var myobj = JSON.parse(dateString);
		player.setData(myobj);
	},
	
	LoadYG: function ()
	{
		//LoadCloud();
		console.log("LoadCloud");
		player.getData().then(_date => {
			let myJSON = JSON.stringify(_date);
			console.log(myJSON);
			myGameInstance.SendMessage("YandexProvider", "SetPlayerProgressData", myJSON);
		})
	},

	GetPlayerData: function () {
		myGameInstance.SendMessage('YandexProvider', 'SetName', player.getName());
		myGameInstance.SendMessage('YandexProvider', 'SetPhoto', player.getPhoto("medium"));
	},

	FullscreenAdv: function () {
		ysdk.adv.showFullscreenAdv({
			callbacks: {
				onClose: function(wasShown) {
          		// some action after close
				},
				onError: function(error) {
          		// some action on error
				}
			}
		})
	},

	RewardedVideo: function () {
		ysdk.adv.showRewardedVideo({
			callbacks: {
				onOpen: () => {
					console.log('Video ad open.');
				},
				onRewarded: () => {
					console.log('Rewarded!');
				},
				onClose: () => {
					console.log('Video ad closed.');
				}, 
				onError: (e) => {
					console.log('Error while open video ad:', e);
				}
			}
		})
	},


	GetStickyAdvSatus: function() {

	// ysdk.adv.getBannerAdvStatus() — показывает статус баннера;
	// ysdk.adv.showBannerAdv() — вызывает баннер;
	// ysdk.adv.hideBannerAdv() — убирает баннер.

		ysdk.adv.getBannerAdvStatus().then(({ stickyAdvIsShowing , reason }) => {
			if (stickyAdvIsShowing) {
        // реклама показывается
			} else if(reason) {
        // реклама не показывается
				console.log(reason)
			} else {
				ysdk.adv.showBannerAdv()
			}
		})
	},

	ReplayRateGame: function() {
		ysdk.feedback.requestReview()
	},

	RateGame: function() {
		ysdk.feedback.canReview()
		.then(({ value, reason }) => {
			if (value) {
				ysdk.feedback.requestReview()
				.then(({ feedbackSent }) => {
					console.log(feedbackSent);
				})
			} else {
				console.log(reason)
			}
		})
	},

	LoadLeaderboards: function(){
		ysdk.getLeaderboards()
		.then(lb => {
    // С использованием всех значений по умолчанию
			lb.getLeaderboardEntries('TestLBYA')
			.then(res => console.log(res));
    // Получение 10 топов
			lb.getLeaderboardEntries('TestLBYA', { quantityTop: 10 })
			.then(res => console.log(res));
    // Получение 10 топов и 3 записей возле пользователя
			lb.getLeaderboardEntries('TestLBYA', { quantityTop: 10, includeUser: true, quantityAround: 3 })
			.then(res => console.log(res));
		});
	},

	SaveLeaderBoards: function(value){
		ysdk.getLeaderboards()
		.then(lb => {
    // Без extraData
			lb.setLeaderboardScore('TestLBYA', value);
    // С extraData
      // lb.setLeaderboardScore('leaderboard2021', 120, 'My favourite player!');
		});
	},

});

var FileIO = {

	SaveToLocalStorage : function(key, data) {
		try {
			localStorage.setItem(UTF8ToString(key), UTF8ToString(data));
		}
		catch (e) {
			console.error('Save to Local Storage error: ', e.message);
		}
	},

	LoadFromLocalStorage : function(key) {
		var returnStr = localStorage.getItem(UTF8ToString(key));
		var bufferSize = lengthBytesUTF8(returnStr) + 1;
		var buffer = _malloc(bufferSize);
		stringToUTF8(returnStr, buffer, bufferSize);
		return buffer;
	},

	RemoveFromLocalStorage : function(key) {
		localStorage.removeItem(UTF8ToString(key));
	},

	HasKeyInLocalStorage : function(key) {
		try {
			if (localStorage.getItem(UTF8ToString(key))) {
				return 1;
			}
			else {
				return 0;
			}
		}
		catch (e) {
			console.error('Has key in Local Storage error: ', e.message);
			return 0;
		}
	}
};

mergeInto(LibraryManager.library, FileIO);
const video = document.getElementById("video");

async function initialize() {

    const params = new URLSearchParams(window.location.search);

    const sessionId = params.get("session");

    if (!sessionId) {

        document.body.innerHTML =
            "<h2>Invalid session.</h2>";

        return;
    }

    if (!navigator.mediaDevices ||
        !navigator.mediaDevices.getUserMedia) {

        document.body.innerHTML =
            "<h2>Your browser does not support camera access.</h2>";

        return;
    }

    try {

        const stream =
            await navigator.mediaDevices.getUserMedia({

                video: {

                    facingMode: {
                        ideal: "environment"
                    },

                    width: {
                        ideal: 1280
                    },

                    height: {
                        ideal: 720
                    }

                },

                audio: false

            });

        video.srcObject = stream;

    }
    catch (error) {

        console.error(error);

        let message = "Unable to access camera.";

        switch (error.name) {

            case "NotAllowedError":
                message = "Camera permission was denied.";
                break;

            case "NotFoundError":
                message = "No camera was found on this device.";
                break;

            case "NotReadableError":
                message = "Camera is already being used by another application.";
                break;

            case "OverconstrainedError":
                message = "Requested camera settings are not supported.";
                break;

            case "SecurityError":
                message = "Camera access was blocked due to browser security.";
                break;
        }

        document.body.innerHTML =
            `<h2>${message}</h2>`;
    }
}

initialize();
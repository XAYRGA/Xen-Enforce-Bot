<?php 
	define('USE_DATABASE',1);
	include 'databank.php';
	
	$secret = "ADD SECRET HERE";
	
	if (!empty($_SERVER['HTTP_CLIENT_IP'])) {
		$ip = $_SERVER['HTTP_CLIENT_IP'];
	} elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
		$ip = $_SERVER['HTTP_X_FORWARDED_FOR'];
	} else {
		$ip = $_SERVER['REMOTE_ADDR'];
	}
	if (isset($_POST['actid'])) {
			$actid = $_POST['actid'];
	} else {
			$actid = "!!NONE3!!";
	}
	if (isset($_POST['g-recaptcha-response'])) {
		 $resp = $_POST['g-recaptcha-response'];
	} else {
		header('Location: ./index.php' . "?success=0&reason=No%20captcha%20token%20received&actid=$actid");
		die("no response from captcha $ip");
	}
	
	//echo ("actid $actid");
	//echo ("gcap $resp");
	
	
// extract data from the post
// set POST variables
$data = array(
            'secret' => "$secret",
            'response' => "$resp"
        );
$verify = curl_init();
curl_setopt($verify, CURLOPT_URL, "https://www.google.com/recaptcha/api/siteverify");
curl_setopt($verify, CURLOPT_POST, true);
curl_setopt($verify, CURLOPT_POSTFIELDS, http_build_query($data));
curl_setopt($verify, CURLOPT_SSL_VERIFYPEER, false);
curl_setopt($verify, CURLOPT_RETURNTRANSFER, true);
$response = curl_exec($verify);
$resp_dec = json_decode((string)$response);
if ($resp_dec->success==false) {
	header('Location: ./index.php' . "?success=0&reason=Recaptcha validation failed&actid=$actid");
} else {

	if (!($stmt = $DB_OBJ->prepare("UPDATE verify SET verified=1,tverified=? WHERE challenge=?"))) {
	    echo "Prepare failed: (" . $mysqli->errno . ") " . $mysqli->error;
	    die();
	}

	if (!$stmt->bind_param("is", time(),$actid)) {
	    echo "Binding parameters failed: (" . $stmt->errno . ") " . $stmt->error;
	    die();
	}
	if (!$stmt->execute()) {
	    echo "Execute failed: (" . $stmt->errno . ") " . $stmt->error;
	    die();
	}
	
	if ($stmt->affected_rows < 1) {
    	// to be successful, this needs to have altered exactly one row
	    // this means that the query succeeded, but it did not alter exactly 1 row
    	die();
	}
	
	header('Location: ./index.php' . "?success=1&reason=Successfully verified that you're human :)&actid=$actid");
}
?>

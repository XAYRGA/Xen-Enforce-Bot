<?php 
	define('USE_DATABASE',1);
	include 'databank.php';
	$secret = "";
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


 if(isset($_POST['h-captcha-response']) && !empty($_POST['h-captcha-response']))
  {
        $secret = 'your_secret_key';
        $verifyResponse = file_get_contents('https://hcaptcha.com/siteverify?secret='.$secret.'&response='.$_POST['h-captcha-response'].'&remoteip='.$_SERVER['REMOTE_ADDR']);
        $responseData = json_decode($verifyResponse);
        if($responseData->success)
        {
            header('Location: ./index.php' . "?success=0&reason=Recaptcha validation failed&actid=$actid");
        }
        else
        {
        	if (!($stmt = $DB_OBJ->prepare("UPDATE verify SET verified=1,tverified=? WHERE challenge=?"))) {
			    echo "Prepare failed: (" . $mysqli->errno . ") " . $mysqli->error;
			    die();
			}
				
			if (!$stmt->bind_param("is",time(), $actid)) {
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
			
			header('Location: ./index.php' . "?success=1&reason=Successfully verified that you're not an evil robot :)&actid=$actid");
        }
   }
?>

{-# LANGUAGE DataKinds #-}
{-# LANGUAGE TypeOperators #-}
{-# LANGUAGE DeriveGeneric #-}
{-# LANGUAGE DeriveAnyClass #-}
{-# LANGUAGE OverloadedStrings #-}

module Main where

import Servant
import Network.Wai.Handler.Warp (run)
import Data.Aeson (ToJSON, FromJSON)
import Data.Text (Text)
import GHC.Generics (Generic)
import System.IO (stdout, hSetBuffering, BufferMode(..))

type API = "health" :> Get '[JSON] HealthResponse
      :<|> "analyze" :> ReqBody '[JSON] AnalyzeRequest :> Post '[JSON] AnalyzeResponse

data HealthResponse = HealthResponse 
    { status :: Text }
    deriving (Show, Eq, Generic, ToJSON)

data AnalyzeRequest = AnalyzeRequest 
    { matchId :: Text }
    deriving (Show, Eq, Generic, FromJSON)

data AnalyzeResponse = AnalyzeResponse 
    { message :: Text
    , processed :: Bool 
    }
    deriving (Show, Eq, Generic, ToJSON)

healthHandler :: Handler HealthResponse
healthHandler = return $ HealthResponse "ok"

analyzeHandler :: AnalyzeRequest -> Handler AnalyzeResponse
analyzeHandler req = return $ AnalyzeResponse 
    { message = "Data received for match: " <> matchId req
    , processed = True 
    }

server :: Server API
server = healthHandler :<|> analyzeHandler

main :: IO ()
main = do
    hSetBuffering stdout NoBuffering
    putStrLn "Starting Analyze Service (Haskell) on port 8002..."
    run 8002 (serve (Proxy :: Proxy API) server)
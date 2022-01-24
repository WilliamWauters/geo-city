import React, { useState, useEffect } from "react";
import {
  Box,
  Grid,
  Backdrop,
  CircularProgress,
  Card,
  CardContent,
  Typography,
  CardHeader,
  Tabs,
  Tab,
} from "@mui/material";
import FavoriteIcon from "@mui/icons-material/Favorite";
import PersonPinIcon from "@mui/icons-material/PersonPin";
import PeopleIcon from "@mui/icons-material/People";
import { Link } from "react-router-dom";

const tabHeight = "40px"; // default: '48px'
const useStyles = {
  grid: {
    minWidth: "400px",
    maxWidth: "80%",
    margin: "auto",
    marginTop: "10px",
    marginBottom: "10px",
  },
  tabsRoot: {
    bgcolor: "#ffffff",
    minHeight: tabHeight,
    height: tabHeight,
  },
  tabRoot: {
    marginBotttom: "10px",
    minHeight: tabHeight,
    height: tabHeight,
    width: "33%",
  },
};

export const TripsManager = (props: {
  component:
    | boolean
    | React.ReactChild
    | React.ReactFragment
    | React.ReactPortal
    | null
    | undefined;
}) => {
  const [open, setOpen] = React.useState(false);
  const [value, setValue] = React.useState("/trips-manager/owner");

  const handleChange = (
    _event: any,
    newValue: React.SetStateAction<string>
  ) => {
    setValue(newValue);
  };

  return (
    <>
      <Grid container>
        <Grid item xs={12}>
          <Box sx={useStyles.grid}>
            <Tabs
              value={value}
              onChange={handleChange}
              aria-label="icon label tabs example"
              centered
              sx={useStyles.tabsRoot}
            >
              <Tab
                icon={<PersonPinIcon />}
                value="/trips-manager/owner"
                component={Link}
                iconPosition="start"
                label="Trips Owner"
                to={"/trips-manager/owner"}
                sx={useStyles.tabRoot}
              />
              <Tab
                icon={<PeopleIcon />}
                value="/trips-manager/participant"
                component={Link}
                iconPosition="start"
                label="Trips Participant"
                to={"/trips-manager/participant"}
                sx={useStyles.tabRoot}
              />
              <Tab
                icon={<FavoriteIcon />}
                value="/trips-manager/favorite"
                component={Link}
                iconPosition="start"
                label="Favorites"
                to={"/trips-manager/favorite"}
                sx={useStyles.tabRoot}
              />
            </Tabs>
          </Box>
        </Grid>
        <Grid item xs={12}>
          <Box sx={useStyles.grid}>{props.component}</Box>
        </Grid>
      </Grid>
      <Backdrop
        sx={{ color: "#fff", zIndex: (theme) => theme.zIndex.drawer + 1 }}
        open={open}
      >
        <CircularProgress color="inherit" />
      </Backdrop>
    </>
  );
};

export default TripsManager;

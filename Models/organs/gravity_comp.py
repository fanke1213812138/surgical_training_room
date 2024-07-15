#!usr/bin/env python

import rospy
from std_msgs.msg import Bool

def main():
    rospy.init_node("gravity_compensation_node")
    pub_left= rospy.Publisher("/dvrk/MTML/set_gravity_compensation",Bool, queue_size=10)
    pub_right= rospy.Publisher("/dvrk/MTMR/set_gravity_compensation",Bool, queue_size=10)

    rate = rospy.Rate(10)

    while not rospy.is_shutdown():
        pub_left.publish(True)
        pub_right.publish(True)
        rate.sleep()


if __name__ == '__main__':
    main()